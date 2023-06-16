class MessagingList{
    public List<Messaging> messaging {get; set;}
}

class Messaging{
    public Dictionary<string, string> sender {get; set;}
    public Dictionary<string, string> recipient {get; set;}
    public Dictionary<string, object> message {get; set;}

    // public long? timestamp {get; set;}
}
class MessResponse{
    public List<MessagingList> entry {get; set;}
    public string getMessage(){
        // return entry[0].messaging[0].message["text"];
        return "";
    }
    

    // public long getTimestamp(){
    //     return entry[0]["messaging"][0]["timestamp"]
    // }

    public string getSenderId(){
        return entry[0].messaging[0].sender["id"];
    }

    public string getReceiverId(){
        return entry[0].messaging[0].recipient["id"];
    }
}

// {"entry":[{"time":1681827428517,"id":"0","messaging":[{"sender":{"id":"12334"},"recipient":{"id":"23245"},"timestamp":"1527459824","message":{"mid":"test_message_id","text":"test_message"}}]}]}

// class TempDto{
//     public List<TempDto1> entry {get; set;} 
// }

// class TempDto1{
//     public long time {get; set;}
// }




// {
//   "field": "messages",
//   "value": {
//     "sender": {
//       "id": "12334"
//     },
//     "recipient": {
//       "id": "23245"
//     },
//     "timestamp": "1527459824",
//     "message": {
//       "mid": "test_message_id",
//       "text": "test_message"
//     }
//   }
// }