# TestTask.Identity
 
This project is used as authorization server in conjunction with project Transaction-API, but can be used as a separate authorization server. Project Transaction-API is used to execute requests to work with the database.
Before testing this project, make sure you have installed and configured Microsoft SQL Server and SSMS.

To testing of this project in conjunction with project Transaction-API:
1. Clone this repository and repository Transaction-API.
2. In files '[Name of Project].DB.Schema/DataContext.cs' and '[Name of Project].Services/appsettings.json/appsettings.Dev.json' in both projects correct the values of 'connectionString' variables: replace "Server = VLAD-NOUT-HP; ..." to "Server = [PC_NAME]; ..." where [PC_NAME] is your PC name.
3. In Visual Studio set the value '[Name of Project].DB.Schema' like Startup Project at Standard Panel and like Default Project at Package Manager Console (View -> Other Windows -> Package Manager Console) in both projects. Execute 'Update-Database' into Package Manager Console in both projects. Return the values of Startup Project and Default Project back to '[Name of Project].Services' in both projects.
4. Run both projects.
5. Go to https://localhost:44368/swagger/index.html
6. Complete the successful 'Register' request by entering the required details. Use a strong password!
7. Make a successful 'Login' request by entering the data specified in p.6.
8. Copy the token from the response (from the 'Login' request).
9. Go to https://localhost:44317/swagger/index.html
10. Click the green 'Authorize' button on the right and paste the token into the appropriate field. Confirm by clicking the green 'Authorize' button and then click 'Close'.

Now you can execute any necessary request from the provided list.

For easy testing of the project, you can first populate the database with values. To do this, it is recommended to use the 'Merge' request in the Transaction-API project with the 'data.csv' file passed to it. This file is located at the root of the Transaction-API repository.
