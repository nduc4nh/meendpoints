using System.Text.Json;
using System.IO.Pipelines;
using System.Buffers;
using System.Text;

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

app.MapPost("/api/v2/message", (MessResponse? messResonse) =>{
    if (messResonse == null ) return Results.StatusCode(403);
    if (messResonse.entry == null || messResonse.entry.Count() == 0) return Results.Problem("Empty data");

    string responseJson = JsonSerializer.Serialize(messResonse);
    string user = messResonse.getReceiverId();
    string time = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");
    File.WriteAllText($"./local_storage/{user}_{time}", responseJson);
    return Results.Ok(messResonse);
});

// app.MapPost("/api/v2/message", (HttpContext ctx) => {

// });

app.MapGet("/get_something", () => {

    return Results.Ok("lsdkfjsldf");
})


app.Run("http://0.0.0.0:5093");
