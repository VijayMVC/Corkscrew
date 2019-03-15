# Corkscrew CMS v1.0
This is the original Corkscrew code-base. Untouched.

## Build pre-requisites
None.

## Build & Run instructions (Dev)

1. Build and publish Corksrew_ConfigDB (SQL Server database project) to a SQL Server. Can be SQL Server 2008 R2 to the absolute latest and even the SQL Azure DB.

2. Now happily run any of the runnable projects:

  * Control Center
  
  * Drive
  
  * Explorer
  
  * Provision Website
  
  * Workflow Service (Windows Service -- when you F5, it will install itself into the local Windows Services)
  
  * Import Export Tool, Install Workflow tool -- both are command-line tools.
  
## Create a working SETUP for installation & distribution
Simply compile the "Setup" project. This will do **everything** for you. At the end of it, there will be a "BuildDrops" folder at the directory where the main SLN file is. Within this, there will be a file "CorkscrewSetup.exe". This is what you need.

## Install instructions
Copy CorkscrewSetup.exe to a temporary folder (desktop), right-click and "Run as Administrator" (it will fail if you don't elevate). Then follow the instructions on-screen. 

We are not using any third-party installer tech, and its all hand-written code. Therefore, you can perform SxS install operations with other installers safely. Just be sure both installers arent trying to modify the exact same folder (not really possible, but...).
