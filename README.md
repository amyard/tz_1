## Run Project
type in terminal    
```
git clone https://amyard@bitbucket.org/amyard/tz_1.git
cd tz_1/Backend
dotnet watch run
```
it will be using https://localhost:5001   

## How to use Postman Collection?    
1. Open Postman
2. Click on Import button -> Upload Files -> Choose TZ_v1.postman_collection.json from Postman folder -> click Import
3. In Collection Tab will be added folder TZ_v1
4. Right Mouse Click on folder TZ_v1 -> click on Edit -> click on Variables -> need add variable URL with value https://localhost:5001  Result must be something like this    
```
VARIALBE |   INITIAL VALUE        |    CURRENT VALUE       |
-----------------------------------------------------------|
url      | https://localhost:5001 | https://localhost:5001 |
```