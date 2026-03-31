# Salon Booking System - Implementation Report

## Project Overview
This project is a console-based application for managing hair salon appointments. It follows a multi-tier architecture to ensure separation of concerns.

## Architecture Layers
- **Models**: Contains the `Appointment` class with 5 attributes (Id, Name, Service, Date, Time).
- **Data (Repository)**: Implements `FileRepository` using the `IRepository` interface to handle CSV storage.
- **Services**: `AppointmentService` handles business logic, such as ensuring the client name is not empty.
- **UI**: `ConsoleUI` provides the interactive English menu for the user.

## Features
- **Create**: Add new appointments with auto-incrementing IDs.
- **Read**: List all appointments or search by a specific ID.
- **Update**: Modify existing appointment details.
- **Delete**: Remove an appointment from the system.
- **Persistence**: All data is saved in `Data/appointments.csv`.