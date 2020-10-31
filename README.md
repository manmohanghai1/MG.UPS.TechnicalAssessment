# MG.UPS.TechnicalAssessment
This project is a Technical Assessment submitted by Manmohan Ghai to UPS.
This is a desktop application which manages the employee details. It uses latest 

The solution has 3 projects
1.	Employee Management WinForm. (MG.UPS.TechnicalAssessment.EmployeeManagement) 
2.	Employee Management Test for Unit tests. Uses MoQ. (MG.UPS.TechnicalAssessment.EmployeeManagementTest
3.	Reusable/independent service client which can interact with rest services. (MG.UPS.TechnicalAssessment.ServiceClientRest)

Functionality implemented
- Add
- Edit
- Delete
- View
- Search
- Export to Csv

The project uses multiple layers to take care of separation of concern. And consume Rest Service to implement mentioned functionality. The functionality uses various patterns and extended base functionality. 

Dependencies

.Net Framework 4.7.2

MG.UPS.TechnicalAssessment.ServiceClientRest
- Microsoft.Extensions.DependencyInjection >= 3.1.9
- Microsoft.Extensions.Options >= v3.1.9
- Microsoft.Extensions.Http >= v3.1.9
- Newtonsoft.Json >= 12.0.3

MG.UPS.TechnicalAssessment.EmployeeManagement
- Newtonsoft.Json >= 12.0.3
- Microsoft.Extensions.DependencyInjection >= 3.1.9

MG.UPS.TechnicalAssessment.EmployeeManagementTest
- Moq >= 4.1.4.7
- System.Net.Http
