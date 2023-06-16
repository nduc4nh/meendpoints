from fastapi import FastAPI
from pymongo import MongoClient

app = FastAPI()

@app.get("/connect-db")
def do_something():
    url = "mongodb+srv://nducanh:Dota2fan@andro.qzdjd.mongodb.net/test"
    client = MongoClient(url)
    db = client.get_database("WRSN")
    col = db["outcome_final"]
    return col.find_one({}).__str__()