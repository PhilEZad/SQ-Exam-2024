
### **CreateBooking Method Decision Table**

**Parameters:**
- `StartDate` (SDV, SDP)
- `EndDate` (EDV, EDP)
- `Room Availability` (RA, RNA)

#### **Equivalence Classes for Parameters:**
- **SDV (StartDate Valid):** StartDate is in the future.
- **SDP (StartDate Past):** StartDate is in the past.
- **EDV (EndDate Valid):** EndDate is after StartDate.
- **EDP (EndDate Past):** EndDate is before StartDate.
- **RA (Room Available):** A room is available for the specified dates.
- **RNA (Room Not Available):** No room is available for the specified dates.

#### **Decision Table for CreateBooking**

| Test Case | StartDate (SDV/SDP) | EndDate (EDV/EDP) | Room Availability (RA/RNA) | Action                                      |
|-----------|---------------------|-------------------|----------------------------|---------------------------------------------|
| TC1       | SDV (Valid)          | EDV (Valid)       | RA (Available)             | Return true (Booking created)              |
| TC2       | SDP (Past)           | EDV (Valid)       | RA (Available)             | Return false (Invalid StartDate)           |
| TC3       | SDV (Valid)          | EDP (Past)        | RA (Available)             | Return false (Invalid EndDate)             |
| TC4       | SDV (Valid)          | EDV (Valid)       | RNA (Not Available)        | Return false (Room not available)          |
| TC5       | SDP (Past)           | EDP (Past)        | RA (Available)             | Return false (Invalid dates)               |
| TC6       | SDV (Valid)          | EDP (Past)        | RNA (Not Available)        | Return false (Invalid EndDate)             |

---

### **FindAvailableRoom Method Decision Table**

**Parameters:**
- `StartDate` (SDV, SDP)
- `EndDate` (EDV, EDP)
- `Room Availability` (RA, RNA)

#### **Equivalence Classes for Parameters:**
- **SDV (StartDate Valid):** StartDate is in the future.
- **SDP (StartDate Past):** StartDate is in the past.
- **EDV (EndDate Valid):** EndDate is after StartDate.
- **EDP (EndDate Past):** EndDate is before StartDate.
- **RA (Room Available):** A room is available for the specified dates.
- **RNA (Room Not Available):** No room is available for the specified dates.

#### **Decision Table for FindAvailableRoom**

| Test Case | StartDate (SDV/SDP) | EndDate (EDV/EDP) | Room Availability (RA/RNA) | Action                                      |
|-----------|---------------------|-------------------|----------------------------|---------------------------------------------|
| TC1       | SDV (Valid)          | EDV (Valid)       | RA (Available)             | Return room ID                              |
| TC2       | SDV (Valid)          | EDV (Valid)       | RNA (Not Available)        | Return -1 (No available room)              |
| TC3       | SDV (Valid)          | EDP (Past)        | RA (Available)             | Throw ArgumentException (Invalid EndDate)  |
| TC4       | SDP (Past)           | EDV (Valid)       | RA (Available)             | Throw ArgumentException (Invalid StartDate)|
| TC5       | SDV (Valid)          | EDP (Past)        | RA (Available)             | Throw ArgumentException (Invalid dates)    |

---

### **GetFullyOccupiedDates Method Decision Table**

**Parameters:**
- `StartDate` (SDV, SDP)
- `EndDate` (EDV, EDP)
- `Fully Occupied Dates` (FO, NFO)

#### **Equivalence Classes for Parameters:**
- **SDV (StartDate Valid):** StartDate is in the future.
- **SDP (StartDate Past):** StartDate is in the past.
- **EDV (EndDate Valid):** EndDate is after StartDate.
- **FO (Fully Occupied):** Some dates within the range are fully occupied.
- **NFO (Not Fully Occupied):** No dates within the range are fully occupied.

#### **Decision Table for GetFullyOccupiedDates**

| Test Case | StartDate (SDV/SDP) | EndDate (EDV/EDP) | Fully Occupied (FO/NFO) | Action                                      |
|-----------|---------------------|-------------------|--------------------------|---------------------------------------------|
| TC1       | SDV (Valid)          | EDV (Valid)       | FO (Some Occupied)        | Return list of fully occupied dates        |
| TC2       | SDV (Valid)          | EDV (Valid)       | NFO (None Occupied)       | Return empty list                          |
| TC3       | SDP (Past)           | EDV (Valid)       | FO (Some Occupied)        | Throw ArgumentException (Invalid StartDate)|
| TC4       | SDV (Valid)          | EDP (Past)        | FO (Some Occupied)        | Throw ArgumentException (Invalid EndDate)  |

---
