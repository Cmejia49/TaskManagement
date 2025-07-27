
1 Update Connection String 

<connectionStrings>
  <add name="DefaultConnection" connectionString="YOUR_SQL_CONNECTION_STRING" providerName="System.Data.SqlClient" />
</connectionStrings>

2 Create Task Table 

IF OBJECT_ID(N'dbo.Tasks', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tasks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Pending', 'InProgress', 'Completed')),
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END


3 Build the solution

4 Open in Visual Studio and Build > Build Solution

5 Run the project

POSTMAN Curl 

GET
curl --location --request GET 'https://localhost:44370/api/tasks/' \
--header 'Content-Type: application/json' 

POST
curl --location 'https://localhost:44370/api/tasks' \
--header 'Content-Type: application/json' \
--data '{
  "title": "Test",
  "description": "description",
  "status": "Pending"
}'

PUT 

curl --location --request PUT 'https://localhost:44370/api/tasks/1' \
--header 'Content-Type: application/json' \
--data '{
  "title": "update",
  "description": "description",
  "status": "Pending"
}'

DELETE

curl --location --request DELETE 'https://localhost:44370/api/tasks/1' \
--header 'Content-Type: application/json' 

GET By Id
curl --location --request GET 'https://localhost:44370/api/tasks/2' \
--header 'Content-Type: application/json'