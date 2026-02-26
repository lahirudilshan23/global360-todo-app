# TODO Application (Angular + .NET Web API)

This is a simple TODO list application built as part of a technical assessment.

The application allows users to:
- Log in using JWT authentication
- View TODO items
- Add new TODO items
- Edit existing TODO items
- Delete TODO items
- Receive UI feedback via Material snackbars

The backend stores data in memory only (no database).

---
### Hard-coded test credentials

- Username: admin
- Password: password

## Tech Stack

### Frontend
- Angular version 21.0.0
- Angular Material version 21.0.5
- TypeScript

### Backend
- .NET Web API (.NET 8.0)
- JWT Authentication (Access Token + Refresh Token)
- In-memory data storage

---

## Authentication

The application uses JWT-based authentication.
