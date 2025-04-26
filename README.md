# Chat Application with React, GraphQL, and Confluent Cloud  

This project showcases my ability to build a real-time chat application by integrating diverse technologies into a cohesive solution. It highlights my expertise in backend development, cloud infrastructure, and basic frontend implementation.

## Features  
- **Chat Application**: Users can join the chat by entering their name, send messages, and view real-time messages from others.  
- **Two Servers**:  
    1. **Producer Server**:  
         - Publishes messages to Confluent Cloud topics using GraphQL (via the HotChocolate library in C#).  
         - Deployed using AWS App Runner.  
    2. **Consumer Server**:  
         - Subscribes to Confluent Cloud topics and broadcasts messages to all connected clients using SignalR.  
         - Deployed on an AWS EC2 instance.  

## Technologies Used  
### Primary Learning Goals  
1. **Confluent Cloud**  
2. **GraphQL**  
3. **React** (basic implementation)  

### Additional Tools and Skills  
1. **AWS** 
    
    * `AWS Amplify`: For single-page application deployment (CI/CD).  
    * `AWS App Runner`: For GraphQL server and producer (CI/CD via GitHub Actions by deploying Docker images to ECR).  
    * `EC2`: For the consumer and SignalR-based chat hub (partial CI/CD).  
    * `Route 53`: For DNS management.  
    * `Parameter Store`: For securely storing secrets (e.g., Confluent Cloud API keys).  
    
2. **GitHub Actions** for CI/CD  
3. **SignalR**  
4. **C#**  

### Challenges Faced  
1. **Real-Time Communication**: Implementing SignalR for seamless real-time updates.   
2. **Cloud Integration**: Configuring Confluent Cloud and securely managing API keys.  
3. **CI/CD Pipelines**: Setting up automated deployment pipelines using GitHub Actions and AWS services.  
4. **Setting up an SSL Certificate**: Using Route 53 and ACM (AWS Certificate Manager) was a challenging task.

## Future Improvements  
If given more time, I plan to: 

1. Backend:

     1. Refactor the project structure and improve naming conventions.  
     2. Add authentication to restrict access to the app.  
     3. Write unit tests to enhance code quality.  
     4. Use Terraform for Infrastructure as Code (IaC).  
     5. Explore additional improvements as needed.

2. Frontend:

     1. Make the code more modular.  
     2. Use the Apollo Client library for GraphQL.  
     3. Add a better CSS library.

## Architecture Diagram  
![Architecture Diagram](screenshots/ArchitectureDiagram.drawio.png)  

## How It Works  
1. **Producer Server**:  
     - Users send messages via GraphQL, which are published to a Confluent Cloud topic.  

2. **Consumer Server**:  
     - The consumer server listens to the Confluent Cloud topic for new messages and broadcasts them to all connected clients using SignalR.  

This project showcases my ability to integrate multiple technologies and deploy a functional application.  

## Screenshots

### Login screen on web and mobile

![Login Screen on web and Mobile](screenshots/LoginScreenOnDifferentDevice.png)  

### Chat app interface on web and mobile

![Chat app look Web and Mobile](screenshots/HostedChatApp.png)
