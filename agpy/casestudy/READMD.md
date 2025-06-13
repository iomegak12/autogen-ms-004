# Customer Service Chat Bot Demo

## Overview

This project demonstrates a customer service chat bot designed to handle a variety of support requests. The bot provides an interactive interface for users to:

- **Inquire about product information**
- **Place new orders**
- **Track the status of existing orders**
- **Register complaints**

The goal is to showcase how conversational AI can streamline customer support processes in a modern application.

## Features

- **Product Information Inquiry:**  
  Users can ask questions about available products, features, and specifications.

- **Order Placement:**  
  The bot guides users through placing new orders, collecting necessary details interactively.

- **Order Status Tracking:**  
  Users can check the status of their orders by providing order details.

- **Complaint Registration:**  
  Customers can register complaints or issues, which are logged for follow-up.

## Technology Stack

- **FastAPI:**  
  Provides a robust backend API for handling chat interactions and business logic.

- **AutoGen:**  
  Powers the conversational AI capabilities, enabling dynamic and context-aware responses.

- **Streamlit:**  
  Delivers a user-friendly web interface for interacting with the chat bot in real time.

## Data Storage

For demonstration purposes, all data (such as products, orders, and complaints) is stored in-memory using Python **lists** and **dictionaries**.  
**Note:** This approach is suitable for demos and testing, but not for production use.

## Getting Started

1. **Clone the repository**
2. **Install dependencies**  
   Make sure you have Python 3.8+ installed.
   ```sh
   pip install -r requirements.txt
   ```
3. **Run the FastAPI backend**
   ```sh
   uvicorn main:app --reload
   ```
4. **Start the Streamlit frontend**
   ```sh
   streamlit run app.py
   ```
5. **Interact with the chat bot**  
   Open the Streamlit app in your browser and start chatting!

## Limitations

- Data is not persisted between sessions.
- The application is for demonstration purposes only.

## License

This project is provided for educational and demonstration purposes.
