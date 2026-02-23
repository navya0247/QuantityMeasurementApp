# 📏 Quantity Measurement Application (C# .NET)

## 📌 Project Overview

The **Quantity Measurement Application** is a console-based C# (.NET) project designed to demonstrate **Object-Oriented Programming principles**, **clean architecture**, and **Test-Driven Development (TDD)** using real-world measurement scenarios.

This project simulates a professional software development workflow where features are implemented incrementally using **Use Cases (UC1 → UC6...)**, each adding new capabilities without breaking existing functionality.

The system allows users to:

* Compare different length measurements
* Convert units seamlessly
* Perform arithmetic operations on measurements
* Maintain accuracy across multiple unit types


# 🧠 Core Concepts Demonstrated

This project teaches important software engineering concepts:

### 🔹 Object-Oriented Programming

* Classes & Objects
* Method Overriding
* Encapsulation
* Value Objects

### 🔹 Clean Architecture

* Separation of Models, Services, and UI
* Single Responsibility Principle

### 🔹 Enum-Driven Design

* Centralized unit conversion logic
* Easy scalability

### 🔹 Test Driven Development

* MSTest framework
* Incremental test coverage

### 🔹 Mathematical Accuracy

* Floating-point precision handling
* Base unit normalization

---

# 🏗️ Project Structure


QuantityMeasurementApp
│
├── Models
│   ├── Feet.cs
│   ├── Inches.cs
│   ├── QuantityLength.cs
│   └── LengthUnit.cs
│
├── Services
│   └── QuantityMeasurementService.cs
│
├── Program.cs
│
└── QuantityMeasurementApp.Tests
    └── QuantityMeasurementTests.cs


---

# 🚀 Implemented Use Cases

---

# ✅ UC1 — Feet Equality

## 🎯 Goal

Compare two **Feet** values for equality.

## ✔ Features

* Feet value object created
* Custom `Equals()` logic
* Null and reference checks

## 📌 Example

```
Input: 1 ft and 1 ft
Output: Equal = TRUE
```

---

# ✅ UC2 — Inches Equality

## 🎯 Goal

Compare two **Inches** values for equality.

## ✔ Features

* Separate Inches class
* Same equality logic pattern as Feet
* Maintains backward compatibility

---

# ✅ UC3 — Generic Length Equality

## 🎯 Goal

Compare different length units using a **single generic class**.

## ✔ Features

* Introduced `QuantityLength`
* Added `LengthUnit` enum
* Conversion to base unit (Feet)
* Cross-unit equality

## 📌 Example

```
1 foot == 12 inches → TRUE
```

---

# ✅ UC4 — Extended Units (Yards & Centimeters)

## 🎯 Goal

Add more units without modifying core logic.

## ✔ Features

* Added YARDS and CENTIMETERS to enum
* Conversion factors centralized
* No code duplication

## 📌 Supported Conversions

```
1 Yard = 3 Feet = 36 Inches
1 cm = 0.393701 Inches
```

---

# ✅ UC5 — Unit Conversion Feature

## 🎯 Goal

Convert between any two length units.

## ✔ Features

* Static conversion API
* Base unit normalization
* Precision handling
* Validation for invalid inputs

## 📌 Example

```
convert(1 ft → inches) = 12
convert(3 yards → feet) = 9
```

---

# ✅ UC6 — Addition of Length Units

## 🎯 Goal

Add two length values even if units differ.

## ✔ Features

* Converts to base unit first
* Returns result in first operand's unit
* Maintains immutability
* Handles negative and zero values

## 📌 Examples

```
1 ft + 12 inches = 2 ft
12 inches + 1 ft = 24 inches
1 yard + 3 ft = 2 yards
```

---

# 🧪 Testing

The project includes **comprehensive MSTest coverage**:

* Same-unit equality tests
* Cross-unit equality tests
* Conversion tests
* Edge cases (zero, negative, large values)
* Arithmetic addition tests

Total coverage includes:

✔ UC1 → UC6 scenarios
✔ Null checks
✔ Precision tolerance

Run tests using:

```
dotnet test
```

---

# ▶️ Running the Application

### Build

```
dotnet build
```

### Run

```
dotnet run
```

The menu allows users to:

* Compare Feet
* Compare Inches
* Compare Generic Lengths
* Convert Units
* Add Length Measurements

---

# 🎯 Key Design Advantages

## ✅ Scalable Architecture

New units can be added by updating only the enum.

## ✅ DRY Principle

Single conversion logic reused everywhere.

## ✅ Accuracy

All calculations normalized to a base unit.

## ✅ Clean Code

Separation between Models, Services, and UI.

---

# 👩‍💻 Technologies Used

* C# (.NET 10)
* MSTest Framework
* Visual Studio / VS Code
* Git & GitHub

---








