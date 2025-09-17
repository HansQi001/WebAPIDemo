# Web API Demo

This project is hosted on **Azure App Service**, with **Azure API Management** (APIM) acting as the gateway.

---

## 🔑 Authentication

At runtime, a default user is created:

- **Username:** `Hans`  
- **Password:** `1234`  

You can use these credentials to fetch a **JWT token** from:

```
POST /api/Auth/Login
```

---

## 📖 Swagger UI

You can explore and test the API through Swagger UI:

- **URL:** [Swagger UI](https://webapidemo-g8hhepfng7dqa2hx.australiaeast-01.azurewebsites.net/)  
- To authenticate:
  1. Open the **Auth** section in Swagger.
  2. Use the following JSON payload to fetch a JWT token:
     ```json
     {
       "username": "Hans",
       "password": "1234"
     }
     ```
  3. Copy the token and click **Authorize** in Swagger UI to set it.

---

## 📬 Access via Postman

When accessing the API through **Postman** (via API Management):

- **Base URL:**  
  ```
  https://apimanagerdemo.azure-api.net
  ```

- **Required Headers:**
  | Key                         | Value                                                |
  |------------------------------|------------------------------------------------------|
  | `Ocp-Apim-Subscription-Key` | `abd4a7cbf38a40b6914d522ac78b1ba4`                   |
  | `Authorization`             | `Bearer [your-jwt-token]`                            |

- **Steps:**
  1. Fetch a JWT token from:
     ```
     POST https://apimanagerdemo.azure-api.net/api/Auth/Login
     ```
  2. Add both headers above to your Postman request.
  3. Call any API endpoint listed in Swagger UI.  
     Example:
     ```
     GET https://apimanagerdemo.azure-api.net/api/Products
     ```

---

## 🚀 Example Workflow

1. **Login** → Obtain JWT token using `Hans/1234`.  
2. **Authorize** → Add `Authorization: Bearer [token]` header.  
3. **Access APIs** → Call endpoints like `/api/Products` via APIM or Swagger.

---

✅ Now your API is ready to test through either **Swagger UI** or **Postman**.
