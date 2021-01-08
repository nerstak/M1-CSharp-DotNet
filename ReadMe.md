# M1-CSharp-DotNet

## Table of contents

- [Informations](#Informations)

- [Features](#Features)

- [Installation](#Installation)

- [Usage](#Usage)

## Informations

### Description

This project is a network chat. It is composed of 3 parts:

- 1 server

- 1 common library

- 2 clients (1 GUI and 1 CLI)

### Technologies used

Built with:

- C#

- NetCore 3.1 / NetFramework 4.7.2

- WPF (Windows Presentation Foundation)

IDE:

- JetBrains Rider

## Features

This project allows user to:

- Log in and log out

- Create topics

- Connect and leave topics (several at the same time)

- Send messages to a topic

- Send private messages to a user

## Installation

Each part of the project (clients, library, server) has its own solution.

Firstly, open the Communication Library and set it up to build in NetCoreApp 3.1 and NetFramework 4.7.2 at the same time.

If other projects don't recognize them directly, add the NetCore library to ClientText and Server projects, and NetFramework to ClientGui, as reference.

## Usage

### Presaved users:

- admin / admin

- julia / julia

- jean / jean

### Commands

- `/join-topic nameTopic` Join nameTopic

- `/leave-topic nameTopic` Leave  nameTopic

- `list-topic` List topics on the server

- `/create-topic nameTopic` Create nameTopic

- `/pm nameUser message` Send message to nameUser

- `/tell nameTopic message` Send message to nameTopic

- `/logout` Logout
