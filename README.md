# Chat Application with React, GraphQL, and Confluent Cloud  

This project demonstrates my skills in building a real-time chat application.  

## Features  
- **Chat Application**: Users can join the chat by entering their name, broadcast messages, and view messages from others in real-time.  
- **Two Servers**:  
    1. **Producer Server**:  
         - Sends messages to Confluent Cloud topics using GraphQL (HotChocolate library in C#).  
         - Deployed using AWS App Runner.  
    2. **Consumer Server**:  
         - Listens to Confluent Cloud topics and broadcasts messages to all clients using SignalR.  
         - Deployed on an AWS EC2 instance.  

## Technologies Used  
### Primary Learning Goals  
1. **Confluent Cloud**  
2. **GraphQL**  
3. **React** (basic implementation)  

### Additional Tools and Skills  
1. **AWS** 
    
    * `AWS amplify`: for Single page application deployment (CI/CD)
    * `AWS App runner`:  for GraphQL Sever and Producer (Ci/CD via Github actions by deploying Docker image to ECR)
    * `EC2`: for Consumer and SignalR based Chathub (partial CI/CD)
    * `Route 53`: for DNS records
    * `Parameter Store`: To save secrets (e.g. Confluent Cloud API keys)
    
2. **GitHub Actions** for CI/CD  
3. **SignalR**  
4. **C#**  


## Future Improvements  
If given more time, I plan to: 

1. Backend:

     1. Refactor the project structure and improve naming conventions.  
     2. Add authentication to restrict access to the app.  
     3. Write unit tests for better code quality.  
     4. Use Terraform for Infrasctuctur as Code
     5. And many more.

2. Frontend:

     1. Make the code more modular
     2. Use Appolo client library for GraphQL
     3. Add better CSS library

## Architecture Diagram  
![Architecture Diagram](screenshots/ArchitectureDiagram.drawio.png)  


## How It Works  
1. **Producer Server**:  
     - Users send messages via GraphQL.  
     - Messages are published to a Confluent Cloud topic.  

2. **Consumer Server**:  
     - Listens to the Confluent Cloud topic for new messages.  
     - Broadcasts messages to all connected clients using SignalR.  

This project showcases my ability to integrate multiple technologies and deploy a functional application.  


## Screenshots

### Login Screen on web and Mobile

![Login Screen on web and Mobile](screenshots/LoginScreenOnDifferentDevice.png)  

### Chat app look Web and Mobile

![Chat app look Web and Mobile](screenshots/HostedChatApp.png)  