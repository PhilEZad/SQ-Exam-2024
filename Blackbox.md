# Test Plan for `BookingManager`

## **Method: `CreateBooking`**

### **Test 1: Successfully create a booking**
- **Input**:
    - Booking with `StartDate` as tomorrow and `EndDate` two days later.
- **Expected Output**:
    - `true`
- **Verification**:
    - Booking is added to the repository with an assigned `RoomId`.
    - `IsActive` is set to `true`.

---

### **Test 2: Fail to create a booking when no room is available**
- **Input**:
    - Booking with `StartDate` and `EndDate` overlapping fully occupied dates.
- **Expected Output**:
    - `false`

---

### **Test 3: Fail to create a booking when `StartDate` is in the past**
- **Input**:
    - Booking with `StartDate` as yesterday.
- **Expected Output**:
    - `ArgumentException` thrown.

---

### **Test 4: Fail to create a booking when `StartDate` > `EndDate`**
- **Input**:
    - Booking with `StartDate` as tomorrow and `EndDate` as today.
- **Expected Output**:
    - `ArgumentException` thrown.

---

## **Method: `FindAvailableRoom`**

### **Test 1: Find an available room successfully**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: two days later.
- **Precondition**:
    - At least one room has no bookings overlapping the given period.
- **Expected Output**:
    - A valid `RoomId`.

---

### **Test 2: No available room found**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: two days later.
- **Precondition**:
    - All rooms have overlapping bookings during the given period.
- **Expected Output**:
    - `-1`.

---

### **Test 3: Invalid date range (StartDate in the past)**
- **Input**:
    - `StartDate`: yesterday.
    - `EndDate`: two days later.
- **Expected Output**:
    - `ArgumentException` thrown.

---

### **Test 4: Invalid date range (StartDate > EndDate)**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: today.
- **Expected Output**:
    - `ArgumentException` thrown.

---

## **Method: `GetFullyOccupiedDates`**

### **Test 1: Retrieve fully occupied dates**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: seven days later.
- **Precondition**:
    - At least one date in the range is fully occupied (all rooms are booked).
- **Expected Output**:
    - A list of fully occupied dates.

---

### **Test 2: No fully occupied dates**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: seven days later.
- **Precondition**:
    - No date in the range has all rooms booked.
- **Expected Output**:
    - An empty list.

---

### **Test 3: Invalid date range (StartDate > EndDate)**
- **Input**:
    - `StartDate`: seven days later.
    - `EndDate`: tomorrow.
- **Expected Output**:
    - `ArgumentException` thrown.

---

### **Test 4: Fully occupied dates when there are no rooms**
- **Input**:
    - `StartDate`: tomorrow.
    - `EndDate`: seven days later.
- **Precondition**:
    - The `roomRepository` is empty.
- **Expected Output**:
    - An empty list.
