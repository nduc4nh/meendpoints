using System.Text.Json;
using System.IO.Pipelines;
using System.Buffers;
using System.Text;
using Azure.Storage.Files.DataLake;
using Azure.Storage;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// subscribe Apis
app.MapGet("/api/v2/message", (HttpContext httpContext) => {
    // read credential
    Dictionary<string, string>? localCred =  JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("./config_file.json"));
    if (localCred == null) return Results.Problem("Internal Exception");
    string cred = localCred["verifyToken"];
    var query = httpContext.Request.Query;

    // read request params
    string? mode = query["hub.mode"];
    string? token = query["hub.verify_token"];
    string? challenge = query["hub.challenge"];

    // validate request
    if (mode == "subscribe" && token == cred){
        return Results.Text(challenge);
    } 

    return Results.StatusCode(403);

});

// app.MapPost("/api/v2/message", (MessResponse? messResonse) =>{
//     if (messResonse == null ) return Results.StatusCode(403);
//     if (messResonse.entry == null || messResonse.entry.Count() == 0) return Results.Problem("Empty data");

//     string responseJson = JsonSerializer.Serialize(messResonse);
//     string user = messResonse.getReceiverId();
//     string time = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");
//     File.WriteAllText($"./local_storage/{user}_{time}", responseJson);
//     return Results.Ok(messResonse);
// });


app.MapPost("/api/v2/message", (MessResponse? messResonse) =>{
    if (messResonse == null ) return Results.StatusCode(403);
    if (messResonse.entry == null || messResonse.entry.Count() == 0) return Results.Problem("Empty data");

    string responseJson = JsonSerializer.Serialize(messResonse);
    string user = messResonse.getReceiverId();
    string time = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");
    // File.WriteAllText($"./local_storage/{user}_{time}", responseJson);
    // return Results.Ok(messResonse);
    String accessUrl = "https://phatstrg.blob.core.windows.net/datalake-fake?sp=racwdlmeop&st=2023-06-16T07:54:22Z&se=2023-06-30T15:54:22Z&spr=https&sv=2022-11-02&sr=c&sig=p%2FaDP%2Fes57gSphbdHGZmmiS1sGgTv%2B7OUFKab9Y33hY%3D";
    String accessToken = "tIJqbVlUU8uOdfTNoXRlGMeNGG+/WycgbBTQTO9tUPnSb4dX0x7TqN4IHkGbHNSFVh4qV99mjNaq+AStlnVdUg==";
    //Specify client credential
    StorageSharedKeyCredential cred = new StorageSharedKeyCredential("phatstrg", accessToken);
    String dfsUri = "https://" + "phatstrg" + ".dfs.core.windows.net";
    
    //Create File System client using DataLakeServiceClient
    DataLakeServiceClient serviceClient = new DataLakeServiceClient(new Uri(dfsUri), cred);
    DataLakeFileSystemClient fsClient = serviceClient.GetFileSystemClient("datalake-fake");

    DataLakeDirectoryClient dirClient = fsClient.GetDirectoryClient("landing");
    if (!dirClient.Exists()) dirClient.Create();

    DataLakeDirectoryClient conversationDirClient  = dirClient.GetSubDirectoryClient("conversation");
    if (!conversationDirClient.Exists()) conversationDirClient.Create();

    DataLakeDirectoryClient userDirClient = conversationDirClient.GetSubDirectoryClient(user); 
    if (!userDirClient.Exists()) userDirClient.Create();

    DataLakeDirectoryClient userDateDirClient = userDirClient.GetSubDirectoryClient(time); 
    if (!userDateDirClient.Exists()) userDateDirClient.Create();

    DataLakeFileClient fileClient = userDateDirClient.GetFileClient("user_conversation.json");
    // fileClient.Create();

    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseJson));
    fileClient.Upload(stream, true);
    
    return Results.Ok("Transfer successfully!");
});




// app.MapPost("/api/v2/message", (HttpContext ctx) => {

// });

// app.MapGet("/get_something", () => {

//     return Results.Ok("lsdkfjsldf");
// })


app.Run("http://0.0.0.0:5093");
