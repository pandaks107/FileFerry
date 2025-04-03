# # FileFerry - Automated File Processing

Functional requirement

1.The FileFerry project needs to copy files from Source Network to Archive location.
2.Move Files from Archive Location to Destination Location
3.Delete Source File if file sent to destination successfully.
4.Use FileCommand as a workflow. (Please refer attached FileCommand.cs File) For example - you can create a List of FileCommand to execute workflow to copy, move and Delete file.
5.Log all file activity to track the files.


## 📌 Project Overview
FileFerry is a .NET 8 Console Application that automates file operations such as **copying, moving, and deleting files** between network locations. It uses **Serilog for logging**, reads configurations from `appsettings.json`, and follows a structured workflow to process files efficiently.

## 🚀 Features
- **Automated File Processing Workflow** (Copy, Move, Delete)
- **Configuration-based Paths** (Source, Archive, Destination)
- **Serilog Logging** (Console + File Logging)
- **Network Path Handling**


---

## 🏗️ Project Structure
```
FileFerry/                 # Root directory
│── appsettings.json       # Configuration file (paths & logging settings)
│── Program.cs             # Main entry point - loads config & executes workflow
│── FileCommand.cs         # Handles individual file operations
│── FileProcessor.cs       # Executes the workflow of file operations
│── README.md              # Project documentation
│── LoggerConfig/          # Logging
```

---

## ⚙️ Setup & Installation
### 1️⃣ **Clone the Repository**
```bash
git clone <https://github.com/pandaks107/FileFerry>
cd FileFerry
```


## 🛠 Configuration (`appsettings.json`)
The application reads paths and logging settings from `appsettings.json`:

```json
{
  "NetworkPaths": {
    "Source": "networkpath/Source",
    "Archive": "networkpath/Archive",
    "Destination": "networkpath/Destination"
  },
  "Logging": {
    "LogFilePath": "Networkpath"
  }
}
```

### 🔹 **Assumptions**
1. The `Source`, `Archive`, and `Destination` folders exist within in network

Example : Network path created Root path: \\MSI\Networkpath

        "Source": "\\\\MSI\\Networkpath\\Source",
        "Archive": "\\\\MSI\\Networkpath/Archive",
        "Destination": "\\\\MSI\\Networkpath/Destination",
        "Filename": "file1.txt"

2. Files are moved in a sequential **workflow (Copy → Move → Delete)**.
3. The application has **read/write permissions** to the network paths.
4. The logging directory (`logs/`) is automatically created.
5. The filenames are **case-sensitive**, ensuring proper execution.

---

## 📌 File Processing Workflow
1️⃣ **Copy** file from `Source` → `Archive`

2️⃣ **Move** file from `Archive` → `Destination`

3️⃣ **Delete** file from `Source` if it successfully reaches `Destination`

```markdown
![Step 1: Screenshot](images/networkPath.png)



## 📝 Logs
Logs are stored in `\\MSI\Networkpath` and include detailed **file operations, warnings, and errors**.

**Example Log Output:**
```
[17:51:55 INF] FileFerry application started...
[17:51:55 INF] Starting file processing workflow...
[17:51:57 INF] Copied: \\MSI\Networkpath/Archive\file1.txt -> \\MSI\Networkpath\Source\file1.txt
[17:51:57 INF] Moved: \\MSI\Networkpath\Source\file1.txt -> \\MSI\Networkpath/Destination\file1.txt
[17:51:57 INF] Deleted: \\MSI\Networkpath\Source\file1.txt
[17:51:57 INF] Workflow execution completed.
File processing completed. Check logs for details.
```

---

## 🤖 Future Enhancements
- Add **parallel processing** for improved performance
- Implement **retry logic** for network failures ( using Library Polly)
- Support **custom file filters** (e.g., process only `.txt` files)
- Azure sdk for the Blob storage 
- Azure App config and Key Vault to store configuration and secrets 

---

## ❓ Troubleshooting
✅ **Issue:** "File not found in networkpath"
- Ensure paths are correctly configured in `appsettings.json`
- Check file permissions
- Verify the network location is accessible

✅ **Issue:** "Unauthorized Access Exception"
- Run the application as an **administrator**
- Ensure the user has permissions to access the network paths




## 📧 Contact
For questions, reach out at *pandaks107@gmail.com**.

