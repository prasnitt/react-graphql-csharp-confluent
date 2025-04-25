import React, { useRef, useState } from "react";
import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import { request, gql } from "graphql-request";

const signalRUrl = import.meta.env.VITE_SIGNALR_URL;
const graphQLUrl = import.meta.env.VITE_GRAPHQL_URL;

interface ChatMessage {
  sender: string;
  content: string;
  timestamp: string;
}

const Chat: React.FC = () => {
  const [userName, setUserName] = useState("");
  const [passPhrase, setPassPhrase] = useState("");
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [isConnected, setIsConnected] = useState(false);
  const [authError, setAuthError] = useState<string | null>(null);
  const [chatError, setChatError] = useState<string | null>(null);
  const connectionRef = useRef<HubConnection | null>(null);

  const connectToSignalR = async (name: string) => {
    const connection = new HubConnectionBuilder()
      .withUrl(`${signalRUrl}?name=${name}`, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (message: ChatMessage) => {
      setMessages((prev) => [...prev, message]);
    });

    connection.onclose((error) => {
      console.error("SignalR disconnected", error);
      setIsConnected(false);
    });

    await connection.start();
    connectionRef.current = connection;
    setIsConnected(true);
  };

  const sendMessage = async () => {
    if (!message.trim()) return;

    const mutation = gql`
      mutation SendMessage($sender: String!, $content: String!, $passPhrase: String!) {
        sendMessage(sender: $sender, content: $content, passPhrase: $passPhrase)
      }
    `;

    try {
      await request(graphQLUrl, mutation, {
        sender: userName,
        content: message,
        passPhrase,
      });
      setMessage("");
    } catch (error: any) {
      console.error("GraphQL error", error);
      const message = error.response?.errors?.[0]?.message || "Failed to send message.";
      setChatError(message);
    }
  };

  const handleJoin = async () => {
    if (!userName.trim() || !passPhrase.trim()) {
      setAuthError("Both fields are required.");
      return;
    }

    const mutation = gql`
      mutation {
        isAuthenticated(passPhrase: "${passPhrase}")
      }
    `;

    try {
      const response = await request(graphQLUrl, mutation);
      if (response.isAuthenticated) {
        await connectToSignalR(userName);
      } else {
        setAuthError("Incorrect passphrase. Please try again.");
      }
    } catch (error) {
      console.error("GraphQL error", error);
      setAuthError("Authentication failed. Try again.");
    }
  };

  if (!isConnected) {
    return (
      <div className="join-form" style={{ display: 'flex', flexDirection: 'column', gap: '1rem', maxWidth: '300px', margin: '0 auto' }}>
        {authError && <p style={{ color: "red" }}>{authError}</p>}
        <h2>Welcome to Chatapp</h2>
        <input
          type="text"
          placeholder="Your name"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              handleJoin();
            }
          }}
          style={{ padding: '10px', borderRadius: '5px', border: '1px solid #ccc' }}
        />
        <input
          type="password"
          placeholder="Passphrase"
          value={passPhrase}
          onChange={(e) => setPassPhrase(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              handleJoin();
            }
          }}
          style={{ padding: '10px', borderRadius: '5px', border: '1px solid #ccc' }}
        />
        <button
          onClick={handleJoin}
          style={{
            padding: '10px',
            backgroundColor: '#007bff',
            color: '#fff',
            border: 'none',
            borderRadius: '5px',
            cursor: 'pointer',
          }}
        >
          Join Chat
        </button>
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
        {chatError && <div style={{ color: "red" }}>{chatError}</div>}
      </div>
      <div className="input-box">
        <input
          type="text"
          placeholder="Type a message..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              sendMessage();
            }
          }}
        />
        <button onClick={sendMessage}>Send</button>
      </div>
    </div>
  );
};

export default Chat;
