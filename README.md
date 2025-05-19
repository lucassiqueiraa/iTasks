# 🛠️ Kanban Task Management System

A lightweight internal application for managing programmer tasks using the **Kanban method** with lists: `ToDo`, `Doing`, and `Done`.

## 📌 Project Summary

Designed for **Managers** and **Programmers**, this app supports task control, user management, and productivity analysis.

- Programmers execute tasks in defined order
- Managers assign tasks and monitor progress
- Role-based access with login system
- All data persisted via **SQL Server** using **Entity Framework**

---

## 👥 Roles & Permissions

| Action                            | Manager ✅ | Programmer ✅ |
|----------------------------------|------------|----------------|
| Login                            | ✔️         | ✔️             |
| Create/Edit/Delete Users         | ✔️         | ❌             |
| Create/Edit Tasks                | ✔️         | ❌             |
| Move Tasks (ToDo → Doing → Done) | ❌         | ✔️ (only own)  |
| View All Tasks                   | ✔️         | ✔️             |
| View Reports                     | ✔️         | ✔️ (only completed) |
| Export Tasks to CSV              | ✔️         | ❌             |

---

## 🧩 Core Features

- 🧑 Unique login with user roles
- 🗂️ CRUD for users and task types (Manager only)
- 📋 Kanban board with 3 lists (ToDo, Doing, Done)
- 🧠 Task order enforcement (1, 2, 3…)
- ⏱️ Real vs estimated duration tracking
- 📈 StoryPoints-based time estimation algorithm
- 📤 CSV export of completed tasks

---

## 📂 Main Forms

- 🔐 **Login**
- 📋 **Kanban Board**
- 👤 **User Management**
- 🧾 **Task Type Management**
- 📝 **Task Details**
- 🔍 **Ongoing Tasks** (Manager)
- ✅ **Completed Tasks** (Both roles)

---

## 🛠️ Tech Stack

- `.NET / C#`
- `Windows Forms or WPF`
- `Entity Framework`
- `SQL Server`
- `CSV Export`

---


