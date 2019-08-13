# SFX100 Profile Converter

## Installation

See [releases](https://github.com/ffxf/sfx100-profile-conv/releases) for further information.

## Convert v0.8.x SimFeedback Profiles to the v0.9.x Format

The upcoming SimFeedback v0.9 release for the [SFX-100 motion platform](https://opensfx.com/) 
has a new format for the XML car profile files. 
This little CLI tools helps to convert the older format into the new one.

## Usage

### Options

    -i, --input       Required. Input file name.
    -o, --output      Output file name. Same as -i if not set but then requires -f.
    -s, --src-dir     Source directory path.
    -d, --dest-dir    Destination directory path.
    -q, --Quiet       (Default: false) Run silently.
    -f, --Force       (Default: false) Force overwriting input file.
    --help            Display this help screen.
    --version         Display version information.

### Examples

    # Create converted coolprofile.xml taken from SFBv0.8 profile with path in your local dir
    <exec_path>/Profile-Converter.exe -i <v0.8path>/profiles/coolprofile.xml
    
    # Create converted coolprofile.xml taken from SFBv0.8 profiles dir in your local dir
    <exec_path>/Profile-Converter.exe -i coolprofile.xml -s <sfb08_path>/profiles
    
    # Create converted newprofile.xml vom coolprofile.xml in current dir
    <exec_path>/Profile-Converter.exe -i coolprofile.xml -o newprofile.xml
    
    # Overwrite coolprofile.xml in converted format - only recommened if working on a copy of the profile
    <exec_path>/Profile-Converter.exe -i coolprofile.xml -f

Converting the complete profiles directory can be done easily, e.g. using PowerShell:

     # Executed from root directory in which SFBv0.8, the converter directory and
     # the destination directory are installed.
     # SFBv0.8 is in subdirectory 'SFBv08'
     # Profile converter has been unzipped into 'Profile-Converter'
     # A directory 'ConvProfs' has been created before executing
     foreach ($f in Get-ChildItem -Path SFBv08\profiles ) { Profile-Converter\Profile-Converter.exe -i $f.name -d ConvProfs }

## Known Issues

Given the tool will likely only been used for a short period of time no attempt was made to catch all potential errors
that might emerge in case of inproper use.

Your v0.8 profile will not be modified unless you use the `-f`/ `--Force` option. As always when touching data you 
do not want to loose it is recommended, however, that you make copies of your old profiles before using this tool.

## Developer Notes

Requires to add [CommandLine Parser](https://www.nuget.org/packages/CommandLineParser/) from NuGet.
