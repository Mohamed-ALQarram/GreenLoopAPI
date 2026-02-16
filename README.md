# GreenLoop
# GreenLoop API Documentation

This document outlines the available API endpoints for the GreenLoop application.

## 🚚 Driver Operations

Base URL: `/api/driver`

### 1. Update Driver Status
Wait for tasks or mark yourself as busy.

- **Endpoint**: `PUT /status`
- **Body**:
  ```json
  {
    "status": "Available" // or "Busy"
  }
  ```
- **Response**:
  ```json
  {
    "message": "Status updated successfully."
  }
  ```

### 2. Get Assigned Tasks
Retrieve tasks assigned to the logged-in driver for a specific date (defaults to today).

- **Endpoint**: `GET /tasks`
- **Query Parameters**:
  - `date`: "today" or "YYYY-MM-DD" (optional)
  - `status`: "Assigned" (optional, default filter in service)
- **Response**:
  ```json
  [
    {
      "id": 1,
      "status": "Assigned",
      "scheduledDate": "2023-10-27T10:00:00Z",
      "customerName": "John Doe",
      "address": "Cairo, 123 Nile St"
    }
  ]
  ```

### 3. Start Task
Mark a task as "InProgress".

- **Endpoint**: `POST /tasks/{id}/start`
- **Response**:
  ```json
  {
    "message": "Task started."
  }
  ```

### 4. Complete Task
Complete a task by submitting weight and waste details. Calculates points earned.

- **Endpoint**: `POST /tasks/{id}/complete`
- **Body**:
  ```json
  {
    "actualWeight": 10.5,
    "wasteCategoryIds": [1, 2],
    "proofPhoto": "base64_string_or_url" // Optional implementation detail
  }
  ```
- **Response**:
  ```json
  {
    "points": 5
  }
  ```

---

## 💰 Customer Wallet & Gamification

Base URL: `/api/wallet`

### 1. Get Wallet Balance
Retrieve the current points balance and total points ever earned.

- **Endpoint**: `GET /balance`
- **Response**:
  ```json
  {
    "currentBalance": 150,
    "totalEarned": 500
  }
  ```

### 2. Get Available Coupons
List coupons available for redemption.

- **Endpoint**: `GET /coupons`
- **Response**:
  ```json
  [
    {
      "couponId": 1,
      "title": "10% Off Coffee",
      "partnerName": "Starbucks",
      "requiredPoints": 100,
      "imageUrl": "http://example.com/image.jpg"
    }
  ]
  ```

### 3. Redeem Coupon
Redeem points for a coupon.

- **Endpoint**: `POST /redeem`
- **Body**:
  ```json
  {
    "couponId": 1
  }
  ```
- **Response**:
  ```json
  {
    "couponCode": "CPN-1-123-ABCD1234",
    "remainingBalance": 50
  }
  ```

### 4. Get Transaction History
View history of points earned and coupons redeemed.

- **Endpoint**: `GET /transactions`
- **Query Parameters**:
  - `page`: Page number (default: 1)
- **Response**:
  ```json
  [
    {
      "type": "Earned",
      "points": 50,
      "description": "Recycled Plastic",
      "date": "2023-10-26T14:30:00Z"
    },
    {
      "type": "Redeemed",
      "points": -100,
      "description": "Redeemed coupon: 10% Off Coffee",
      "date": "2023-10-27T09:15:00Z"
    }
  ]
  ```
