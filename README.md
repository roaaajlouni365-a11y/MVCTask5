# University Cafeteria Inventory and Purchase Management

Run `Database/CafeteriaDB.sql` in SQL Server Management Studio first. It creates the manually defined Database First database and its five required tables.

The application applies the Code First migration to the separate `CafeteriaEmployeesDB` database when it starts. Both connection strings are in `appsettings.json`.

Set an employee username and password before running the application:

```powershell
$env:EmployeeAccount__Username = "employee"
$env:EmployeeAccount__Password = "your-password"
dotnet run
```

The inventory and purchase order pages require employee sign-in. Create or update categories, suppliers, and products from Inventory. A purchase order can contain multiple products; its total uses each product's current price. Mark a pending order as received to add its quantities to inventory.
