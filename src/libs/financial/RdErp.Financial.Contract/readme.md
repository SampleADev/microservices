Examples of events generate transactions:

- Receiving a money from the customer
  Transaction:

  - type: income
  - account: EP USD Account
  - contractor: Best Customer Inc.
  - code: Income -> Customers -> Project payments

- Paying a salary from enterprise balance
  Transaction:

  - type: outcome
  - account: EP UAH Account
  - contractor: John Doe
  - code: Outcome -> Salary -> Monthly Employee

- Exchange an USD income to UAH
  Transaction "spending USD" [1]:

  - type: outcome
  - account: EP USD Account
  - contractor: none
  - code: Exchange -> Selling USD

  Transaction: "receiving UAH":

  - type: income
  - account: EP UAH Account
  - contractor: none
  - code: Exchange -> Buying UAH
  - correlated transaction: [1]

* Paying an HR agency from own cash money
  Transaction:

  - type: outcome
  - account: Trump Cash
  - contractor: none
  - code: Expenses -> HR

* Paying a rent for an office
  Transaction:
  - type: outcome
  - account: Melany Cash
  - contractor: none
  - code: Expenses -> Office -> Rent
