Here is your data formatted into markdown tables:

### CreateBooking Method

**Equivalence Classes:**

| Class | Description |
|-------|-------------|
| SDV   | StartDate is valid (in the future). |
| SDP   | StartDate is in the past. |
| EDV   | EndDate is valid (after StartDate). |
| EDP   | EndDate is before StartDate. |
| RA    | Room is available for the given period. |
| RNA   | Room is not available for the given period. |

**Decision Table for CreateBooking**

| Conditions                          | SDV (StartDate valid) | SDP (StartDate in the past) | EDV (EndDate valid) | EDP (EndDate before StartDate) | RA (Room available) | RNA (Room not available) | Action |
|--------------------------------------|-----------------------|----------------------------|---------------------|--------------------------------|---------------------|--------------------------|--------|
| C1: Valid dates, Room available     | Yes                   | No                         | Yes                 | Yes                            | Yes                 | No                       | Return true (Booking created) |
| C2: Valid dates, Room not available | Yes                   | No                         | Yes                 | Yes                            | No                  | Yes                      | Return false (No available room) |
| C3: Valid StartDate, invalid EndDate | Yes                   | No                         | No                  | Yes                            | -                   | -                        | Return false (Invalid dates) |
| C4: Past StartDate, valid EndDate   | No                    | Yes                        | Yes                 | Yes                            | -                   | -                        | Return false (Invalid dates) |
| C5: Valid StartDate, EndDate before StartDate | Yes           | No                         | No                  | Yes                            | Yes                 | Yes                      | Return false (Invalid dates) |
| C6: Past StartDate, EndDate before StartDate | No            | Yes                        | No                  | Yes                            | -                   | -                        | Return false (Invalid dates) |

---

### FindAvailableRoom Method

**Equivalence Classes:**

| Class | Description |
|-------|-------------|
| SDV   | StartDate is valid (in the future). |
| SDP   | StartDate is in the past. |
| EDV   | EndDate is valid (after StartDate). |
| EDP   | EndDate is before StartDate. |
| RA    | Room is available for the given period. |
| RNA   | Room is not available for the given period. |

**Table for FindAvailableRoom**

| Conditions                                        | SDV (StartDate valid) | SDP (StartDate in the past) | EDV (EndDate valid) | EDP (EndDate before StartDate) | RA (Room available) | RNA (Room not available) | Action |
|--------------------------------------------------|-----------------------|----------------------------|---------------------|--------------------------------|---------------------|--------------------------|--------|
| C1: Valid StartDate and EndDate, Room available  | Yes                   | No                         | Yes                 | Yes                            | Yes                 | No                       | Return room ID |
| C2: Valid StartDate and EndDate, Room not available | Yes                   | No                         | Yes                 | Yes                            | No                  | Yes                      | Return -1 (No available room) |
| C3: Valid StartDate, invalid EndDate             | Yes                   | No                         | No                  | Yes                            | -                   | -                        | Throw ArgumentException |
| C4: Past StartDate, valid EndDate                | No                    | Yes                        | Yes                 | Yes                            | -                   | -                        | Throw ArgumentException |
| C5: Valid StartDate, EndDate before StartDate    | Yes                   | No                         | No                  | Yes                            | -                   | -                        | Throw ArgumentException |

---

### GetFullyOccupiedDates Method

**Equivalence Classes:**

| Class | Description |
|-------|-------------|
| SDV   | StartDate is valid (in the future). |
| SDP   | StartDate is later than EndDate. |
| EDV   | EndDate is valid (after StartDate). |
| FO    | At least one date in the range is fully occupied. |
| NFO   | No date in the range is fully occupied. |

**Decision Table for GetFullyOccupiedDates**

| Conditions                                        | SDV (StartDate valid) | SDP (StartDate later than EndDate) | EDV (EndDate valid) | FO (Fully occupied dates) | NFO (No fully occupied dates) | Action |
|--------------------------------------------------|-----------------------|-----------------------------------|---------------------|---------------------------|--------------------------------|--------|
| C1: Valid dates, some fully occupied dates      | Yes                   | No                                | Yes                 | Yes                        | No                             | Return list of fully occupied dates |
| C2: Valid dates, no fully occupied dates        | Yes                   | No                                | Yes                 | No                         | Yes                            | Return empty list |
| C3: StartDate later than EndDate                | No                    | Yes                               | Yes                 | -                          | -                              | Throw ArgumentException |
| C4: Valid StartDate, invalid EndDate            | Yes                   | No                                | No                  | Yes                        | -                              | Throw ArgumentException |

---
