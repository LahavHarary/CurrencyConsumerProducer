# CurrencyConsumerProducer
A C# .Net Core project that retrieves, saved to db and present data regarding currency rates.
<br><br/>
The code consists of 2 MicroServices and a shared library.

## ms_currency
Micro Service which handles retrieving and saving the data to a sqlite database.
![ms_currency](https://user-images.githubusercontent.com/70967259/234648120-b6375e4c-00c4-4e7b-a8e9-4293980a3ae2.jpeg)

## ms_reader
Micro Service which handles pulling the data from the database and presenting it to the user.
![ms_reader](https://user-images.githubusercontent.com/70967259/234648058-ecf5c950-365c-4ca2-bfb3-97e325bb99fe.jpeg)

## shared_resources
![shared_resources](https://user-images.githubusercontent.com/70967259/234646687-d5439339-6d10-493c-b7a8-f5c66d95657e.jpeg)

## Why did I choose to work with Sqlite for a consumer-prodcuer problem? 
After reading about the consumer - producer problem I figured that instead of solving it on my own I can look for services that have encoutnred it.
A little google search showed me that SQLite has managed to solve this issue. 
<br><br/>
SQLite supports three different Threading modes:
<br><br/>
1. Single-thread. In this mode, all mutexes are disabled and SQLite is unsafe to use in more than a single thread at once. (Not so great for our case).
<br><br/>
2. Multi-thread. In this mode, SQLite can be safely used by multiple threads provided that no single database connection is used simultaneously in two or more threads. (Doable but needs code adaptaion on our end in order to do it properly).
<br><br/>
3. Serialized. In serialized mode, SQLite can be safely used by multiple threads with no restriction. (Solved exactly what we need).
<br><br/> 
After reading the documentation I discovered that SQLite is working by default with Serialized.


## Resources

[SQLite](https://choosealicense.com/licenses/mit/)
