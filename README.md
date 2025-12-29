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

## Endpoints

### OData

#### Azure resource

Microsoft offers a json version of the API which provides more details about the feed then the XML version.

- https://www.microsoft.com/releasecommunications/api/v2/azure

Take from the top

- https://relcomms-prod-dagnegedescbeefs.b02.azurefd.net/api/v2/azure/?$orderby=created%20desc

Take next page from the top

- https://relcomms-prod-dagnegedescbeefs.b02.azurefd.net/api/v2/azure/?$orderby=created%20desc&$skip=100

#### Microsoft 365 resource (Same as above)

- https://www.microsoft.com/releasecommunications/api/v2/m365

For example:

```json
{
  "id": "101-services-at-fedramp-high-as-azure-government-continues-expanding-capacity",
  "productCategories": [
	"Storage",
	"Management and governance",
	"Analytics",
	"Databases",
	"Migration",
	"Security",
	"Networking",
	"Hybrid + multicloud",
	"Internet of Things"
  ],
  "tags": [
	"Compliance",
	"Features",
	"Services"
  ],
  "products": [
	"AI Enrichment",
	"Archive Storage",
	"Azure AI Speech",
	"Azure Blueprints",
	"Azure Data Explorer",
	"Azure Database Migration Service",
	"Azure Dedicated HSM",
	"Azure Firewall",
	"Azure Managed Applications",
	"Azure Resource Graph",
	"Azure Stack Edge",
	"Azure Stream Analytics",
	"Cloud Shell",
	"Microsoft Cost Management",
	"Network Watcher"
  ],
  "generalAvailabilityDate": "2020-03",
  "previewAvailabilityDate": null,
  "privatePreviewAvailabilityDate": null,
  "title": "101 services at FedRAMP High as Azure Government continues expanding capacity",
  "description": "description goes here",
  "status": "Launched",
  "created": "2020-03-19T22:00:17.0000000Z",
  "modified": "2020-03-19T22:00:17.0000000Z",
  "locale": null,
  "availabilities": [
	{
	  "ring": "General Availability",
	  "year": 2020,
	  "month": "March"
	}
  ]
}
```



### Filtering

`https://www.microsoft.com/releasecommunications/api/v2/azure?$filter=modified ge 2025-01-01 and modified ie 2025-01-01`