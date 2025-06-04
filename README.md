The most common naming convention for SQL stored procedures is:

**[schema].[sp/usp/verb_Object/Action]**

- Prefix with `sp` or `usp` (optional, but common: `usp` = user stored procedure).
- Use PascalCase or snake_case.
- Start with a verb describing the action (e.g., `Get`, `Insert`, `Update`, `Delete`).
- Follow with the object or entity name.

**Examples:**

- `dbo.usp_GetCustomerById`
- `dbo.usp_InsertOrder`
- `dbo.sp_UpdateProductStock`
- `dbo.GetEmployeeList`

**Tips:**

- Avoid using the `sp_` prefix (reserved by SQL Server for system procedures).
- Be descriptive and consistent.
Include the schema name (e.g., `dbo.`).

**Recommended:**  
`dbo.usp_ActionObject` (e.g., `dbo.usp_GetOrderDetails`)