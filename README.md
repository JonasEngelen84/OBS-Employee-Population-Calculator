Employee Population Calculator


✨ Description

This project visualizes company locations and employee distributions using OpenStreetMap and Leaflet.
It calculates population circles based on employee data and displays them dynamically on an interactive map.


🔍 Features

	- OpenStreetMap integration with Leaflet.js
	- Dynamic company location display
	- Employee distribution visualization using population circles
	- Configurable properties via appsettings.json
	

🖥️ Languages

	- C#	82.3%
	- HTML	13.8%
	- CSS	3.3%
	- JS	0.6%


🔧 Technologies

	- .NET Core
	- ASP.NET Razor Pages
	- Leaflet.js
	- Geolocation
	- IdentityModel
	- Nominatim
	- System.Linq.Dynamic.Core
	
	
🔼 Installation

Prerequisites:

	- .NET SDK installed
	- Node.js (optional, if client-side dependencies are required)
	- Git (if cloning from GitHub)

Setup Instructions:

	1. Clone the repository
	2. Restore dependencies: dotnet restore
	3. Configure appsettings.json (see Configuration for sensitive data handling)
	4. Run the application
	
	
⚙️ Configuration

Sensitive information such as API keys, database credentials,
and OAuth secrets should not be hardcoded.

Instead, use:

	appsettings.json (excluded in .gitignore)
	Environment variables
	User secrets (dotnet user-secrets)

Example appsettings.json structure:

{
  "Company": {
    "Latitude": 0.0,
    "Longitude": 0.0,
    "Address": "Your Company Address"
  },
  "Authentication": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}


▶️ Running the Application

Once the application is running, http://localhost:5000 should open.


🔒 Security Considerations

	- Never commit sensitive data to the repository.
	- Use .gitignore to exclude configuration files containing secrets.
	- Regularly rotate API keys and credentials.


⚖ License

This project is licensed under the proprietary license. See the LICENSE file for details.


🤝 Contributing

Contributions are welcome! Please fork the repository and submit a pull request.


📧 Contact

For any inquiries just open an issue in the repository.
