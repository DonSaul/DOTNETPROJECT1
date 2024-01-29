# .NET Project LAB API

## Contents
1. [Introduction](#1-introduction) 
2. [Getting Started](#2-getting-started) <br>
   2.1. [Prerequisites](#21-prerequisites) <br>
   2.2. [Initial Setup and Configuration](#22-initial-setup-and-configuration) 
3. [Basic Usage](#3-basic-usage) <br>
   3.1. [Basic Operations and Examples](#31-basic-operations-and-examples) 
4. [Project Architecture](#4-project-architecture) <br>
   4.1. [Model-View-Controller (MVC)](#41-model-view-controller-mvc-pattern) <br>
   4.2. [Service Pattern](#42-service-pattern) <br>
   4.3. [Dependency Injection](#43-dependency-injection) 
5. [Security Features and Best Practices](#5-security-features-and-best-practices) <br>  
6. [Advanced Functionalities](#6-advanced-functionalities) <br>
   6.1. [API Connection](#61-api-connection) <br>
   6.2. [Work Order Filtering](#62-work-order-filtering) <br>
   6.3. [Search Technician By Name](#63-search-technician-by-name) <br>
   6.4. [Report Generation](#64-report-generation)<br>
   6.5. [Testing](#65-testing) <br>
   6.6. [Frontend](#66-frontend)<br>
7. [Error Handling](#7-error-handling) <br>
   7.1 [Try Catches](#71-try-catches)<br>
   7.2 [Input Validation](#72-input-validation)<br>
   
8. [Contribution](#8-contribution)
9. [License](#9-license)

## 1. Introduction
This project offers a streamlined approach to managing and querying Work Orders, along with the Technicians linked to them. Designed with focus on functionality and ease of use, the system offers advanced capabilities for filtering and searching information, thereby enhancing operational efficiency. The project's standout features include:

**Work Order Queries:** This feature enables comprehensive searches for Work Orders. Users can filter and explore data using various criteria.

**Technician Search:** A key aspect of the project is the capability to locate Technicians and display their related Work Orders, aiding in rapid personnel identification.

**Customized Report Generation:** The system is equipped to compile data into well-organized, structured reports.

In terms of architecture, the project consists of two primary segments: Back-end and Front-end, designed to work together to provide a comprehensive solution. The Back-end, developed with .NET technologies, is responsible for communication with the Work Order API and plays a crucial role in the handling and processing of data.  The Front-end, built using Blazor - a .NET framework for creating interactive user interfaces with C# - allows users to interact intuitively with the system's functionalities. This integration facilitates access to and management of Work Orders and Technician information.




# 2. Getting Started
## 2.1 Prerequisites
Before starting the project, it's important to set up the right development environment. This step involves configuring two key components that are essential for a smooth start with the system.

### Docker:

This tool is used for creating and managing containers that bundle applications and services. In this project, Docker is key for setting up a connection to the Work Orders API. To get started, download and install Docker Desktop from the [official Docker Desktop Download website](https://www.docker.com/products/docker-desktop), making sure it's compatible with your operating system. Post-installation, running `docker --version` in the command line is recommended to verify the setup.
### .NET SDK Version 8.0:
Crucial for application development using .NET
technology, .NET SDK Version 8.0 can be obtained from Microsoft's official .NET downloads page [official Docker Desktop Download website](https://dotnet.microsoft.com/es-es/download/dotnet/8.0). After installation, executing `dotnet --version` in the command line is advisable to confirm successful installation.

## 2.2 Initial Setup and Configuration
### Docker Setup
It is crucial to have access to the API that holds Work Orders information. For security and confidentiality concerns, the necessary file to configure Docker Compose is not included in this repository. However, a detailed description of the structure of this file is provided below, ensuring the protection of the API's security.
``` yaml
version: "3"  # Specifies the version of the Docker Compose format being used.

services:
    workorderapi:
        # Use the Docker image of the WorkOrders API.
        image: <api-image> 

        environment:
            # Configures the environment variable for the .NET API to listen on port 80.
          - ASPNETCORE_URLS=http://+:80  
        ports:
            # Connect the container's port 80 to the host's port 80.
          - "80:80"    

        volumes:
            # Mounts the 'sqlite' volume at '/store' path in the container for data persistence.
            - sqlite:/store  

volumes:
     # Defines a volume named 'sqlite' that will be used by the workorderapi service
     sqlite:

```
### Starting the Service
To initialize the service, run docker-compose up. This action will start the necessary service to access the routes containing Work Orders resources, including entities such as WorkOrder, Technician, Status, and WorkType.

### Running the project
Open the .NET solution file in the DEMO2 directory with Visual Studio to open the project. The project, set up as a Web API, supports both HTTP and HTTPS. In Visual Studio, you can choose the running mode (HTTP or HTTPS) for added security in data transmission. This is usually done by selecting the right option in the toolbar to run the application as a Web App.


# 3. Basic Usage
## 3.1 Basic Operations and Examples


Here are some examples of how you can interact with the API:
- **Get all work order**: To get all work order, you can use the following curl command in Postman:
    - curl -X 'GET' \  'https://localhost:7178/api/WorkOrder' \  -H 'accept: application/json'

![Postman Workorder](art/Postman_Workorder.gif)


# 4. Project Architecture
The project uses the MVC, Service, and Dependency Injection design patterns.
## 4.1 Model-View-Controller (MVC) Pattern
The MVC pattern was chosen because it helps separate the application logic, making code management and maintenance easier. This pattern is used to divide an application into three interconnected parts: the model, the view, and the controller.
- The model contains the application data.
- The view presents the data to the user.
- The controller handles the user's interaction with the application.
## 4.2 Service Pattern
The service pattern is used to encapsulate business logic in-service classes. Service classes are used in controllers to perform business logic tasks.
## 4.3 Dependency Injection
The dependency injection pattern is used to provide object dependencies to the objects that need them. Dependency injection allows for looser coupling and more modular code.

# 5. Security features and best practices

# 6. Advanced functionalities
## 6.1 API Connection
## 6.2 Work Order Filtering
This function is designed to retrieve detailed information about work orders based on specific criteria like time period, work type, and status. It leverages data from multiple sources to present a comprehensive view of each work order.
### Functionality Description
**1. Time Frame Filtering:** Filters work orders based on a specified start and end time.

**2. Work Type Filtering:** Allows filtering of work orders based on a specific type of work. Includes an option to select 'all' types.

**3. Status Filtering:** Permits filtering by the status of work orders, with the ability to choose 'all' statuses.

**4. Data Joining:** Joins data from separate entities, such as technicians, work types, and statuses, to form a complete picture of each work order.
### LINQ Usage
The function employs LINQ for its data processing needs, particularly using the following methods:

**1. Join Operations:** Utilizes LINQ's join clause to merge data from technicians, work types, and statuses with work orders.

**2. Where Clause:** Employs where to filter work orders based on the provided criteria, such as time frame, work type, and status.

**3. Select Projection:** Uses select to project the filtered data into the WorkOrderDetails structure.

**Pseudocode Example:**
``` csharp
var query = workOrders
            .Join(technicians, ...)
            .Join(workTypes, ...)
            .Join(statuses, ...)
            .Where(wo => wo.StartTime.HasValue && ...)
            .Select(wo => new WorkOrderDetails { ... });

``` 

## 6.3 Search Technician By Name
Designed to facilitate the querying of a database or dataset to retrieve details about technicians based on their names. It provides a robust and flexible way to handle various types of name-based searches, ensuring accurate and relevant results.
### Functionality Description
**1. Full Name Search:** Allows users to search for technicians by providing a full name (both first and last name). The function returns details for technicians whose names exactly match the input.

**2. First or Last Name Search:** Users can search by either first name or last name. The function will return all technicians matching the given name component.

**3. Case Insensitivity:** The search is case-insensitive, meaning searches for "John Doe," "john doe," or "JOHN DOE" will yield the same results.

**4. Special Character Handling:** The function recognizes and accurately processes names containing special characters (like accents, hyphens, etc.).

**5. Exact Match Requirement:** It requires an exact match of the search term. Hence, partial name searches or searches with typing errors return an empty list.

**6. Unordered Name Search:** The function does not support unordered name searches. If the name components are provided in a different order than what is recorded, the function will not capture those names.


### LINQ Usage
The LINQ expression used in the "Search Technician By Name" function primarily revolves around two key LINQ methods: Where and Select.

**1. Where():** This is used to filter the list of technicians. It checks each technician's name, split into lower-cased words, against the search terms to find an exact match in the correct sequence.

**2. Select():** This projects the filtered technicians into a new form, TechnicianDetails, which includes nested LINQ queries within to fetch related WorkOrderDetails.

The algorithm processes each technician's name to match all search terms in order. It splits the name into words, iterating through them to ensure each search term is found sequentially. Once a technician passes this filter, they are transformed into TechnicianDetails objects, with an additional nested LINQ query fetching their associated WorkOrderDetails.


``` csharp

var filteredTechnicians = technicians
    .Where(t => {
        var techNameWords = t.Name.ToLowerInvariant().Split(' ');
        int searchTermIndex = 0;
        foreach (var word in techNameWords) {
            if (word.Equals(searchTerms[searchTermIndex])) {
                searchTermIndex++;
                if (searchTermIndex == searchTerms.Length) {
                    // All search terms matched in order
                    return true; 
                }
            }
        }
        // Not all search terms matched
        return false;     
})
    .Select(tech => new TechnicianDetails {
        // Project to TechnicianDetails including nested LINQ for WorkOrders
    })
    .ToList();
``` 

## 6.4 Report Generation
Designed to facilitate the creation of comprehensive reports from work order data.
### Functionality Description
**1. Data Retrieval:** The function retrieves all necessary data, which includes details about the work order, such as the technician assigned, the status of the work order, and the type of job being performed.

**2. Data Processing:** Once the data is retrieved, it is processed and joined together to form a comprehensive report. This involves transforming the data into a suitable format and organizing the data in a way that is easy to understand.

**3. CSV Export:** The `WorkOrderController` controller exports the report data as a CSV file. This CSV file can be downloaded and used for further analysis or record-keeping.

**4. Error Handling:** The function includes error handling to manage potential issues with data retrieval or processing, ensuring that the function behaves predictably in all scenarios.


### LINQ Usage

### Remarks

## 6.5 Testing
## 6.6 Frontend

# 7. Error Handling
## 7.1 Try Catches
## 7.2 Input Validation

# 8. Contribution
This project reflects the collective efforts in the Softserve .NET project lab. The focus has been on backend and frontend development, with notable advancements in key areas like Work Order Queries and Technician Search. Utilization of .NET and Blazor technologies has been instrumental in enhancing project functionality. The project has evolved through problem-solving, innovation, and a commitment to learning and improvement. Future enhancements are anticipated to further develop the project's capabilities, ensuring ongoing progress and efficiency.

# 9. License
This source code is made available for educational purposes only. By using this code, you agree not to use it, or any part of it, for commercial purposes. If you wish to use this code for purposes other than education, please contact the author for permission.