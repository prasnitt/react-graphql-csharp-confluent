// src/components/Chat.tsx
import React, { useRef, useState } from "react";
import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import { request, gql } from "graphql-request";


const signalRUrl = "https://localhost:7124/chat-hub"; // server1
const graphQLUrl = "http://localhost:5200/graphql"; // server2

interface ChatMessage {
  sender: string;
  content: string;
  timestamp: string;
}

const Chat: React.FC = () => {
  const [userName, setUserName] = useState("");
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const connectionRef = useRef<HubConnection | null>(null);

  const connectToSignalR = async (name: string) => {
    const connection = new HubConnectionBuilder()
      .withUrl(`${signalRUrl}?name=${name}`,{ withCredentials: true})
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (message: ChatMessage) => {
      setMessages((prev) => [...prev, message]);
    });

    await connection.start();
    connectionRef.current = connection;
  };

  const sendMessage = async () => {
    if (!message.trim()) return;

    const mutation = gql`
      mutation SendMessage($sender: String!, $content: String!) {
        sendMessage(sender: $sender, content: $content)
      }
    `;

    try {
      await request(graphQLUrl, mutation, {
        sender: userName,
        content: message,
      });
      setMessage("");
    } catch (error) {
      console.error("GraphQL error", error);
    }
  };

  const handleJoin = () => {
    if (userName.trim()) {
      connectToSignalR(userName);
    }
  };

  if (!connectionRef.current) {
    return (
      <div className="join-screen">
        <h2>Enter your name to join the chat</h2>
        <input
          type="text"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
        />
        <button onClick={handleJoin}>Join Chat</button>
      </div>
    );
  }

  return (
    <div className="chat-box">
      <h2>Welcome, {userName}!</h2>
      <div className="messages">
        {messages.map((msg, idx) => (
          <div key={idx}>
            <strong>{msg.sender}</strong>: {msg.content}
          </div>
        ))}
      </div>
      <div className="input-box">
        <input
          type="text"
          placeholder="Type a message..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
        />
        <button onClick={sendMessage}>Send</button>
      </div>
    </div>
  );
};

export default Chat;
