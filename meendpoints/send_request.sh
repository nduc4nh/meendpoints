curl -X POST http://dp2:5093/api/v2/message -H "Content-Type: application/json" -d '{"field": "messages", "value": {"sender": {"id": "12334"}, "recipient": {"id": "23245"}, "timestamp": "1527459824", "message": {"mid": "test_message_id","text": "test_message"}}}'  

# // {
# //   "field": "messages",
# //   "value": {
# //     "sender": {
# //       "id": "12334"
# //     },
# //     "recipient": {
# //       "id": "23245"
# //     },
# //     "timestamp": "1527459824",
# //     "message": {
# //       "mid": "test_message_id",
# //       "text": "test_message"
# //     }
# //   }
# // }