<?xml version="1.0"?>
<DTS:Executable xmlns:DTS="www.microsoft.com/SqlServer/Dts"
  DTS:refId="Package"
  DTS:CreationDate="3/1/2010 12:20:29 PM"
  DTS:CreationName="Microsoft.Package"
  DTS:CreatorComputerName="SBDGSQL02"
  DTS:CreatorName="LAWABADGE\b21mmg"
  DTS:DTSID="{4FD51364-1D55-4CF3-B860-7BC07422BD48}"
  DTS:ExecutableType="Microsoft.Package"
  DTS:LastModifiedProductVersion="14.0.3008.28"
  DTS:LocaleID="1033"
  DTS:ObjectName="B2K-ACAMS-Transfer"
  DTS:PackageType="5"
  DTS:VersionBuild="168"
  DTS:VersionGUID="{E0547FCE-776C-4761-8951-1F64BEAF7D49}">
  <DTS:Property
    DTS:Name="PackageFormatVersion">8</DTS:Property>
  <DTS:ConnectionManagers>
    <DTS:ConnectionManager
      DTS:refId="Package.ConnectionManagers[LaxBadgeSql.LAXID]"
      DTS:CreationName="ADO.NET:System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
      DTS:DTSID="{B8841F0C-A4C5-4FA3-AEB3-8277D4823CD6}"
      DTS:ObjectName="LaxBadgeSql.LAXID">
      <DTS:ObjectData>
        <DTS:ConnectionManager
          DTS:ConnectionString="Data Source=laxbadgesql;Initial Catalog=LAXID;Integrated Security=True;Application Name=SSIS-ACAMSTransferPP4-{B8841F0C-A4C5-4FA3-AEB3-8277D4823CD6}LaxBadgeSql.LAXID;" />
      </DTS:ObjectData>
    </DTS:ConnectionManager>
    <DTS:ConnectionManager
      DTS:refId="Package.ConnectionManagers[pp4test]"
      DTS:CreationName="ADO.NET:System.Data.Odbc.OdbcConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
      DTS:DTSID="{B34EBFF8-6889-4772-A18D-E384BCB56EA5}"
      DTS:ObjectName="pp4test">
      <DTS:ObjectData>
        <DTS:ConnectionManager
          DTS:ConnectionString="Dsn=pp45PR;" />
      </DTS:ObjectData>
    </DTS:ConnectionManager>
  </DTS:ConnectionManagers>
  <DTS:Variables />
  <DTS:Executables>
    <DTS:Executable
      DTS:refId="Package\Transfer Person Data"
      DTS:CreationName="Microsoft.ScriptTask"
      DTS:Description="Transfer Person Data"
      DTS:DTSID="{E1000217-A3DC-4903-AA9B-C2EA5261F605}"
      DTS:ExecutableType="Microsoft.ScriptTask"
      DTS:LocaleID="-1"
      DTS:ObjectName="Transfer Person Data"
      DTS:ThreadHint="0">
      <DTS:Variables>
        <DTS:Variable
          DTS:CreationName=""
          DTS:DTSID="{1BDF7E0E-04BF-4D71-BE0A-D673A84F6256}"
          DTS:IncludeInDebugDump="6789"
          DTS:Namespace="User"
          DTS:ObjectName="NumberOfRecords">
          <DTS:VariableValue
            DTS:DataType="3">100</DTS:VariableValue>
        </DTS:Variable>
      </DTS:Variables>
      <DTS:ObjectData>
        <ScriptProject
          Name="ST_fee19305817346c48778eab4d49c9257"
          VSTAMajorVersion="15"
          VSTAMinorVersion="0"
          Language="VisualBasic">
          <ProjectItem
            Name="\my project\settings.settings"
            Encoding="UTF8"><![CDATA[<?xml version='1.0' encoding='iso-8859-1'?>
<SettingsFile xmlns="uri:settings" CurrentProfile="(Default)" GeneratedClassNamespace="$safeprojectname" GeneratedClassName="MySettings">
  <Profiles>
    <Profile Name="(Default)" />
  </Profiles>
  <Settings />
</SettingsFile>]]></ProjectItem>
          <ProjectItem
            Name="st_fee19305817346c48778eab4d49c9257.vbproj"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="utf-16"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <!-- This section defines project-level properties.

       Configuration - Specifies whether the default configuration is Release or Debug.
       Platform - Specifies what CPU the output of this project can run on.
       OutputType - Must be "Library" for VSTA.
       NoStandardLibraries - Set to "false" for VSTA.
       RootNamespace - In C#, this specifies the namespace given to new files.
                       In Visual Basic, all objects are wrapped in this namespace at runtime.
       AssemblyName - Name of the output assembly.
  -->
  <PropertyGroup>
    <ProjectTypeGuids>{30D016F9-3734-4E33-A861-5E7D899E18F3};{F184B08F-C81C-45F6-A57F-5ABD9991F28F}</ProjectTypeGuids>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <RootNamespace>ST_fee19305817346c48778eab4d49c9257.vbproj</RootNamespace>
    <AssemblyName>ST_fee19305817346c48778eab4d49c9257.vbproj</AssemblyName>
    <StartupObject></StartupObject>
    <OptionExplicit>On</OptionExplicit>
    <OptionCompare>Binary</OptionCompare>
    <OptionStrict>Off</OptionStrict>
    <OptionInfer>On</OptionInfer>
    <ProjectGuid>{F995AD3D-4007-43BE-A6C4-845CDAF3D2E9}</ProjectGuid>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <TargetFrameworkProfile></TargetFrameworkProfile>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Debug" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>true</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>true</DebugSymbols>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Release" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>false</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>false</DebugSymbols>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section enables pre- and post-build steps. However,
       it is recommended that MSBuild tasks be used instead of these properties.
  -->
  <PropertyGroup>
    <PreBuildEvent></PreBuildEvent>
    <PostBuildEvent></PostBuildEvent>
  </PropertyGroup>
  <!-- This sections specifies references for the project. -->
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
    <Reference Include="Microsoft.SqlServer.ManagedDTS, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
    <Reference Include="Microsoft.SqlServer.ScriptTask, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
  </ItemGroup>
  <!-- Visual Basic supports Importing namespaces (equivalent to using statements in C#).-->
  <ItemGroup>
    <Import Include="Microsoft.VisualBasic" />
    <Import Include="System" />
    <Import Include="System.Collections" />
    <Import Include="System.Data" />
    <Import Include="System.Diagnostics" />
    <Import Include="System.Windows.Forms" />
  </ItemGroup>
  <!-- This section defines the user source files that are part of the
       project.

       Compile - Specifies a source file to compile.
       EmbeddedResource - Specifies a .resx file for embedded resources.
       None - Specifies a file that is not to be passed to the compiler (for instance,
              a text file or XML file).
       AppDesigner - Specifies the directory where the application properties files can
                     be found.
  -->
  <ItemGroup>
    <AppDesigner Include="My Project\" />
    <Compile Include="My Project\AssemblyInfo.vb">
      <SubType>Code</SubType>
    </Compile>
    <EmbeddedResource Include="My Project\Resources.resx">
      <Generator>VbMyResourcesResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.vb</LastGenOutput>
      <CustomToolNamespace>My.Resources</CustomToolNamespace>
    </EmbeddedResource>
    <Compile Include="My Project\Resources.Designer.vb">
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
      <DependentUpon>Resources.resx</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <None Include="My Project\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.vb</LastGenOutput>
    </None>
    <Compile Include="My Project\Settings.Designer.vb">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="ScriptMain.vb">
      <SubType>Code</SubType>
    </Compile>
    <!-- Include the default configuration information and metadata files for the add-in.
         These files are copied to the build output directory when the project is
         built, and the path to the configuration file is passed to add-in on the command
         line when debugging.
    -->
  </ItemGroup>
  <!-- Include the build rules for a VB project.-->
  <Import Project="$(MSBuildBinPath)\Microsoft.VisualBasic.targets" />
  <!-- This section defines VSTA properties that describe the host-changable project properties. -->
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID="{30D016F9-3734-4E33-A861-5E7D899E18F3}">
        <ProjectProperties HostName="VSTAHostName" HostPackage="{B3A685AA-7EAF-4BC6-9940-57959FA5AC07}" ApplicationType="usd" Language="vb" TemplatesPath="" DebugInfoExeName="" />
        <Host Name="SSIS_ScriptTask" />
        <ProjectClient>
          <HostIdentifier>SSIS_ST140</HostIdentifier>
        </ProjectClient>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
</Project>]]></ProjectItem>
          <ProjectItem
            Name="Project"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="UTF-16" standalone="yes"?>
<c:Project xmlns:c="http://schemas.microsoft.com/codeprojectml/2010/08/main" xmlns:msb="http://schemas.microsoft.com/developer/msbuild/2003" runtimeVersion="4.0" schemaVersion="1.0">
	<msb:PropertyGroup>
		<msb:CodeName>st_fee19305817346c48778eab4d49c9257</msb:CodeName>
		<msb:Language>msBuild</msb:Language>
		<msb:DisplayName>st_fee19305817346c48778eab4d49c9257</msb:DisplayName>
		<msb:ProjectId>{593998D8-F251-46A5-B675-A44FB194988E}</msb:ProjectId>
	</msb:PropertyGroup>
	<msb:ItemGroup>
		<msb:Project Include="st_fee19305817346c48778eab4d49c9257.vbproj"/>
		<msb:File Include="My Project\AssemblyInfo.vb"/>
		<msb:File Include="ScriptMain.vb"/>
		<msb:File Include="My Project\Resources.resx"/>
		<msb:File Include="My Project\Resources.Designer.vb"/>
		<msb:File Include="My Project\Settings.settings"/>
		<msb:File Include="My Project\Settings.Designer.vb"/>
	</msb:ItemGroup>
</c:Project>]]></ProjectItem>
          <ProjectItem
            Name="My Project\Settings.Designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class Settings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As Settings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New Settings),Settings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As Settings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.ST_fee19305817346c48778eab4d49c9257.vbproj.Settings
            Get
                Return Global.ST_fee19305817346c48778eab4d49c9257.vbproj.Settings.Default
            End Get
        End Property
    End Module
End Namespace]]></ProjectItem>
          <ProjectItem
            Name="My Project\Resources.Designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On


Namespace My.Resources
    
    '''<summary>
    '''   A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    'This class was auto-generated by the Strongly Typed Resource Builder
    'class via a tool like ResGen or Visual Studio.NET.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    Class MyResources
        
        Private Shared _resMgr As System.Resources.ResourceManager
        
        Private Shared _resCulture As System.Globalization.CultureInfo
        
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''   Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared ReadOnly Property ResourceManager() As System.Resources.ResourceManager
            Get
                If (_resMgr Is Nothing) Then
                    Dim temp As System.Resources.ResourceManager = New System.Resources.ResourceManager("My.Resources.MyResources", GetType(MyResources).Assembly)
                    _resMgr = temp
                End If
                Return _resMgr
            End Get
        End Property
        
        '''<summary>
        '''   Overrides the current thread's CurrentUICulture property for all
        '''   resource lookups using this strongly typed resource class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared Property Culture() As System.Globalization.CultureInfo
            Get
                Return _resCulture
            End Get
            Set
                _resCulture = value
            End Set
        End Property
    End Class
End Namespace]]></ProjectItem>
          <ProjectItem
            Name="\my project\resources.resx"
            Encoding="UTF8"><![CDATA[<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>]]></ProjectItem>
          <ProjectItem
            Name="ScriptMain.vb"
            Encoding="UTF8"><![CDATA[' Microsoft SQL Server Integration Services Script Task
' Write scripts using Microsoft Visual Basic 2008.
' The ScriptMain is the entry point class of the script.

Imports System
Imports System.Data
Imports System.Math
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.SqlClient
Imports System.Data.Odbc

<Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute> _
<System.CLSCompliantAttribute(False)> _
Partial Public Class ScriptMain
    Inherits Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase

    Enum ScriptResults
        Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success
        Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
    End Enum

    Dim logDir As String = "F:\JobLogs\"    ' TBD parameterize this
    Dim statusMapStr As String = "ACTIVE=0,CONFISCATED=5,EXPIRED=5,INVALID=5,LOST=4,RECALL=5,RETURNED=6,UNCLAIMED=5,CANCELLED=6,STOLEN=4,9-30-02  non FP=5"
    Dim specialCategoriesStartSlot As Integer = 9999    ' mmg: set it to large number to avoid any retention of categories across badge updates
    Dim statusMapTable As Hashtable
    Dim personUserDataMap As New Hashtable
    Dim badgeUserDataMap As New Hashtable


    ' The execution engine calls this method when the task executes.
    ' To access the object model, use the Dts property. Connections, variables, events,
    ' and logging features are available as members of the Dts property as shown in the following examples.
    '
    ' To reference a variable, call Dts.Variables("MyCaseSensitiveVariableName").Value
    ' To post a log entry, call Dts.Log("This is my log text", 999, Nothing)
    ' To fire an event, call Dts.Events.FireInformation(99, "test", "hit the help message", "", 0, True)
    '
    ' To use the connections collection use something like the following:
    ' ConnectionManager cm = Dts.Connections.Add("OLEDB")
    ' cm.ConnectionString = "Data Source=localhost;Initial Catalog=AdventureWorks;Provider=SQLNCLI10;Integrated Security=SSPI;Auto Translate=False;"
    '
    ' Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
    ' 
    ' To open Help, press F1.

    ' Microsoft SQL Server Integration Services Script Task
    ' Write scripts using Microsoft Visual Basic 2008.
    ' The ScriptMain is the entry point class of the script.
    Dim GlobalErrorlevel As Integer = 3
    '  connection attributes
    Dim laxidManager As ConnectionManager
    Dim informixManager As ConnectionManager
    Dim laxidConnection As SqlConnection
    Dim picConnection As SqlConnection
    Dim InformixReadConnection As Odbc.OdbcConnection
    Dim InformixWriteConnection As Odbc.OdbcConnection

    '
    '     stop watch attributes
    'Dim deptTimer As New System.Diagnostics.Stopwatch
    'Dim personTimer As New System.Diagnostics.Stopwatch
    'Dim catTimer As New System.Diagnostics.Stopwatch
    'Dim userTimer As New System.Diagnostics.Stopwatch
    'Dim badgeTimer As New System.Diagnostics.Stopwatch
    '     
    '    counters
    Dim NumberOfDepartmentsAdded As Integer = 0
    Dim numberOfPersonsAdded As Integer = 0
    Dim numberOfBadgesAdded As Integer = 0
    Dim NumberOfPicturesAdded As Integer = 0
    Dim NumberOfPerson_UserAdded As Integer = 0
    Dim NumberOfPerson_Category As Integer = 0

    '    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    'Main routine:
    Public Sub Main()
        ' Initialize badge status map
        Me.statusMapTable = New Hashtable
        Dim spl() As String = Me.statusMapStr.Split(",")
        For Each s As String In spl
            Dim nv() As String = s.Split("=")
            Me.statusMapTable.Add(nv(0), nv(1))
        Next

        badgeUserDataMap.Add("co_name", 1)
        badgeUserDataMap.Add("div_name", 2)
        badgeUserDataMap.Add("color", 3)
        'badgeUserDataMap.Add("user4", 4)
        badgeUserDataMap.Add("customs", 5)
        badgeUserDataMap.Add("job_title", 6)
        'badgeUserDataMap.Add("work_loc", 7)
        badgeUserDataMap.Add("driver", 8)
        badgeUserDataMap.Add("law", 9)
        badgeUserDataMap.Add("gates", 10)
        badgeUserDataMap.Add("atct", 11)

        personUserDataMap.Add("dob", 12)
        personUserDataMap.Add("ssn", 13)
        personUserDataMap.Add("ht_ft", 14)
        personUserDataMap.Add("ht_in", 15)
        personUserDataMap.Add("weight", 16)
        personUserDataMap.Add("sex", 17)
        personUserDataMap.Add("eyes", 18)
        personUserDataMap.Add("hair", 19)
        personUserDataMap.Add("ethnic", 20)
        personUserDataMap.Add("dl_no", 21)
        personUserDataMap.Add("dl_state", 22)
        personUserDataMap.Add("dl_expdt", 23)

        badgeUserDataMap.Add("badgeno", 25)

        Dim refCmd As SqlCommand
        'Dim laxidControlTableReader As SqlDataReader = Nothing
        'If Not IsNothing(Dts.Variables.Item("JobLogDir").Value) Then logDir = Dts.Variables.Item("JobLogDir").Value
        Try
            laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
            laxidConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
            picConnection = laxidManager.AcquireConnection(Nothing) ' Connection for reading picture from laxid
            TraceLog(3, "Aquired connection to LaxBadgeSq")
            'laxidConnection.Open()
            informixManager = Dts.Connections("pp4test")
            InformixReadConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
            InformixWriteConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
            TraceLog(3, "Aquired connection to pp4test")
        Catch ex As Exception
            TraceLog(0, "Exception in connecting to the databases " & ex.ToString)
            Return
        End Try

        Dim c As Integer = 0
        While c < 5000
            c = c + 1
            'refCmd = New SqlCommand("Select TOP 1 * from Transfer_Control where ID=<x>", laxidConnection)
            refCmd = New SqlCommand("Select TOP 1 * from Transfer_Control where End_transmit is null ORDER BY ID ", laxidConnection)
            Dim laxidControlTableReader = refCmd.ExecuteReader()

            If laxidControlTableReader.Read Then
                'Read a record from transfer_control table
                Dim transactionID As Integer = laxidControlTableReader("ID")
                refCmd.Dispose()
                laxidControlTableReader.Close()
                TraceLog(3, "============================Processing row with ID = " & transactionID)
                refCmd = New SqlCommand("UPDATE Transfer_Control Set Start_Transmit=getDate() WHERE ID=@ID", laxidConnection)
                refCmd.Parameters.AddWithValue("ID", transactionID)
                If refCmd.ExecuteNonQuery() <= 0 Then TraceLog(0, "Failed to update start_transmit date for transactionID " & transactionID)
                TransferDivision(transactionID)
                TransferPerson(transactionID)
                TransferCategory(transactionID)
                TransferBadge(transactionID)
                TransferBadgeCategory(transactionID)
                refCmd = New SqlCommand("UPDATE Transfer_Control Set End_Transmit=getDate() WHERE ID=@ID", laxidConnection)
                refCmd.Parameters.AddWithValue("ID", transactionID)
                If refCmd.ExecuteNonQuery() <= 0 Then TraceLog(0, "Failed to update End_transmit date for transactionID " & transactionID)
                refCmd.Dispose()

            Else
                Exit While
            End If
        End While
        laxidConnection.Close()
        InformixReadConnection.Close()
        InformixWriteConnection.Close()
        Dts.TaskResult = ScriptResults.Success
    End Sub
    Sub TransferDivision(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_CompanyDivision where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidDivisionTableReader = refCmd.ExecuteReader

        While laxidDivisionTableReader.Read
            Dim dept As String = "9" + laxidDivisionTableReader("CO_ID").ToString + laxidDivisionTableReader("DIV_ID").ToString
            Dim informixCmd As New Odbc.OdbcCommand("select id from informix.department where division = ?", InformixReadConnection)
            Dim reader As Odbc.OdbcDataReader = Nothing
            informixCmd.Parameters.AddWithValue("division", dept)
            '2. if found exit. 
            Try
                reader = informixCmd.ExecuteReader
                If reader.Read Then
                    TraceLog(3, "Department already exists ID = " & reader("id"))
                    Dim updCmdText As String = "UPDATE informix.department " + _
                                       " SET description=?,location=?,manager=?,phone=?,user1=?,user2=?," + _
                               "facility=?, modify_date=?, modify_time=? WHERE division=?"

                    Dim updCmd As New Odbc.OdbcCommand(updCmdText, InformixWriteConnection)
                    Dim ddesc As String = dept + " " + laxidDivisionTableReader("co_name").trim()
                    If ddesc.Length > 50 Then ddesc = ddesc.Substring(0, 50)
                    updCmd.Parameters.AddWithValue("description", ddesc)
                    Dim dloc As String = laxidDivisionTableReader("CO_Name") + ", " + laxidDivisionTableReader("DIV_Name")
                    If dloc.Length > 60 Then dloc.Substring(0, 60).Trim()
                    updCmd.Parameters.AddWithValue("location", "")
                    updCmd.Parameters.AddWithValue("manager", "")
                    updCmd.Parameters.AddWithValue("phone", "")
                    updCmd.Parameters.AddWithValue("user1", laxidDivisionTableReader("CO_NAME").ToString.Trim)
                    updCmd.Parameters.AddWithValue("user2", laxidDivisionTableReader("DIV_NAME").ToString.Trim)
                    updCmd.Parameters.AddWithValue("facility", -1)
                    updCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    updCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    updCmd.Parameters.AddWithValue("division", dept)
                    Try
                        Dim Nrows As Integer = updCmd.ExecuteNonQuery()
                        If (Nrows = 0) Then
                            TraceLog(1, "Warning: No rows updated by Update department " & dept)
                        Else
                            TraceLog(3, "Department Updated " & dept)
                        End If
                    Catch ex As Exception
                        TraceLog(0, "Casi_Department: Exception updating new department " & dept & ":" & ex.ToString)
                    End Try
                    updCmd.Dispose()
                    informixCmd.Dispose()
                    reader.Close()
                    Continue While
                End If
            Catch ex As Exception
                TraceLog(0, "Casi_Department:Exception in Reading department " & ex.ToString)
            End Try
            informixCmd.Dispose()
            reader.Close()
            ' Create department record in PP
            Dim insCmdText As String = "INSERT INTO informix.department (" + _
                               "description,division,location,manager,phone,user1,user2," + _
                       "facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?,?,?,?)"

            Dim insCmd As New Odbc.OdbcCommand(insCmdText, InformixWriteConnection)
            Dim desc As String = dept + " " + laxidDivisionTableReader("co_name").trim()
            If desc.Length > 50 Then desc = desc.Substring(0, 50)
            insCmd.Parameters.AddWithValue("description", desc)
            insCmd.Parameters.AddWithValue("division", dept)
            Dim loc As String = laxidDivisionTableReader("CO_Name") + ", " + laxidDivisionTableReader("DIV_Name")
            If loc.Length > 60 Then loc.Substring(0, 60).Trim()
            insCmd.Parameters.AddWithValue("location", "")
            insCmd.Parameters.AddWithValue("manager", "")
            insCmd.Parameters.AddWithValue("phone", "")
            insCmd.Parameters.AddWithValue("user1", laxidDivisionTableReader("CO_NAME").ToString.Trim)
            insCmd.Parameters.AddWithValue("user2", laxidDivisionTableReader("DIV_NAME").ToString.Trim)
            insCmd.Parameters.AddWithValue("facility", -1)
            insCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
            insCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
            Try
                Dim Nrows As Integer = insCmd.ExecuteNonQuery()
                If (Nrows = 0) Then
                    TraceLog(1, "Warning: Failed to create department " & dept)
                Else
                    TraceLog(3, "New department created " & dept)
                End If
            Catch ex As Exception
                TraceLog(0, "Casi_Department: Exception creating new department " & dept & ":" & ex.ToString)
            End Try
            insCmd.Dispose()
        End While
        refCmd.Dispose()
        laxidDivisionTableReader.Close()
    End Sub
    Sub TransferCategory(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_Category where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidReader = refCmd.ExecuteReader
        While laxidReader.Read
            Dim informixCmd As New OdbcCommand("UPDATE Category SET description=?,modify_date=?,modify_time=? WHERE id=?", InformixWriteConnection)
            informixCmd.Parameters.AddWithValue("description", laxidReader("descrp"))
            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
            informixCmd.Parameters.AddWithValue("id", laxidReader("CatID"))
            Try
                If informixCmd.ExecuteNonQuery <= 0 Then ' no records updated. Insert record
                    informixCmd.Dispose()
                    informixCmd = New OdbcCommand("INSERT INTO Category (id,description,permission_grp,m2mr_type,facility,modify_date,modify_time)" & _
                                                       " VALUES (?,?,-1,0,?,?,?)", InformixWriteConnection)
                    informixCmd.Parameters.AddWithValue("id", laxidReader("CatID"))
                    informixCmd.Parameters.AddWithValue("description", laxidReader("descrp"))
                    informixCmd.Parameters.AddWithValue("facility", -1)
                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    informixCmd.ExecuteNonQuery()
                    TraceLog(3, "New category created " & laxidReader("CatId"))
                Else ' update was successful
                    TraceLog(3, "Category record updated. id=" & laxidReader("CatId"))
                End If
            Catch ex As Exception
                TraceLog(0, "Exception creating Category ID=" & laxidReader("CatId") & ":" & ex.ToString)
            End Try
            informixCmd.Dispose()
        End While
        refCmd.Dispose()
        laxidReader.Close()
    End Sub

    Sub UpdateInsertCategories(ByVal laxCats As ArrayList, ByVal emp_id As String, ByVal co_div As String)
        ' read existing categories for this person
        Dim sql As String = "SELECT pc.id ID, pc.category_id, pc.slot_number, p.id person_id" & _
                            " from person p" & _
                            " inner join department d on p.department=d.id" & _
                            " left join person_category pc on pc.person_id=p.id" & _
                            " Where p.employee=? and d.division=? Order by pc.category_id"

        Dim informixCmd As New Odbc.OdbcCommand(sql, InformixReadConnection)
        informixCmd.Parameters.AddWithValue("employee", emp_id)
        informixCmd.Parameters.AddWithValue("division", co_div)
        Dim informixReader As OdbcDataReader = informixCmd.ExecuteReader
        Dim currentCats As New Hashtable
        Dim person_id As Integer = 0
        While informixReader.Read
            person_id = informixReader("person_id")
            If informixReader.IsDBNull(informixReader.GetOrdinal("category_id")) Then Continue While ' This will happen if there are no categories
            If informixReader("slot_number") < specialCategoriesStartSlot Then ' not a special category. Retain special categories in PP.
                ' tbd: change this when categories are managed in b2k
                If Not laxCats.Contains(informixReader("category_id").ToString) Then ' category not in new list
                    ' delete this catetory for this person from pp
                    Dim delCommand As New OdbcCommand("DELETE From person_category where person_id=? and category_id=?", InformixWriteConnection)
                    delCommand.Parameters.AddWithValue("person_id", person_id)
                    delCommand.Parameters.AddWithValue("category_id", informixReader("category_id"))
                    delCommand.ExecuteNonQuery()
                    TraceLog(3, String.Format("Person Category {0} Deleted for person {1}", informixReader("category_id"), co_div + "." + emp_id))
                    delCommand.Dispose()
                    Continue While
                End If
            End If
            ' store the category and slot_number for reference during insertion
            If Not currentCats.ContainsKey(informixReader("category_id").ToString) Then currentCats.Add(informixReader("category_id").ToString, informixReader("slot_number").ToString)
        End While
        informixCmd.Dispose()
        informixReader.Close()
        Dim freeSlot As Integer = 0
        Dim personCatMaxID As Integer = 0 ' this will be used as ID during insertion. Should not be required if ID was auto-increment field
        ' insert new category in laxCats i.e. the ones not present in currentCats
        For Each cat As Integer In laxCats
            If Not currentCats.ContainsKey(cat.ToString) Then ' this is new category
                ' find a free slot for insertig category
                Do
                    freeSlot = freeSlot + 1
                Loop While currentCats.ContainsValue(freeSlot.ToString) ' slot number is in use

                If personCatMaxID = 0 Then  ' lazy initialization of max ID
                    informixCmd = New OdbcCommand("SELECT MAX(ID) MaxID from person_category", InformixReadConnection)
                    Integer.TryParse(informixCmd.ExecuteScalar().ToString, personCatMaxID)
                    informixCmd.Dispose()
                End If
                ' insert the new category in pp
                Dim insCommand As New OdbcCommand("INSERT Into person_category (id, person_id, category_id, slot_number, facility, modify_date, modify_time)" & _
                                                  " VALUES (?,?,?,?,?,?,?)", InformixWriteConnection)
                personCatMaxID = personCatMaxID + 1
                TraceLog(3, String.Format("Adding Person_Category id={0}, person_id={1}, slot_number={2}, category_id={3} ", personCatMaxID, person_id, freeSlot, cat))
                insCommand.Parameters.AddWithValue("id", personCatMaxID)
                insCommand.Parameters.AddWithValue("person_id", person_id)
                insCommand.Parameters.AddWithValue("category_id", cat)
                insCommand.Parameters.AddWithValue("slot_number", freeSlot)
                insCommand.Parameters.AddWithValue("facility", -1)
                insCommand.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                insCommand.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                Try
                    If insCommand.ExecuteNonQuery() <= 0 Then
                        TraceLog(1, String.Format("Failed to insert Category {0} for person {1} ", cat, co_div + "." + emp_id))
                    Else
                        TraceLog(3, String.Format("Person Category {0} Added to person {1}", cat, co_div + "." + emp_id))
                    End If
                Catch ex As Exception
                    TraceLog(1, String.Format("Exception Inserting Category {0} for person {1}: {2} ", cat, co_div + "." + emp_id, ex.Message))
                End Try
                insCommand.Dispose()
            End If
        Next
    End Sub
    Sub TransferBadgeCategory(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select distinct emp_id, co_div, category_id from Transfer_BadgeCategory where transferID = @transferID order by emp_id, co_div, category_id", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim emp_id As String = ""
        Dim co_div As String = ""
        Dim laxidReader = refCmd.ExecuteReader
        Dim laxCats As New ArrayList
        Dim moreRecords As Boolean = laxidReader.Read
        While moreRecords
            laxCats.Clear() ' new emp. start over
            emp_id = laxidReader("emp_id")
            co_div = "9" & laxidReader("co_div")
            While moreRecords
                If emp_id.Equals(laxidReader("emp_id").ToString) And co_div.Equals("9" & laxidReader("co_div").ToString) Then
                    laxCats.Add(laxidReader("Category_ID").ToString) ' collect the categories in an array
                    moreRecords = laxidReader.Read
                Else
                    Exit While
                End If
            End While
            UpdateInsertCategories(laxCats, emp_id, co_div)
        End While
        refCmd.Dispose()
        laxidReader.Close()
    End Sub
    Function UpdatePersonRecords(ByVal laxidPersonTableReader As SqlDataReader) As Integer
        ' Update Person based on employee id alone. (note that this may update multiple records)
        Dim informixCmd As Odbc.OdbcCommand
        Dim updSQL = "update informix.person set pin=?" & _
                        ", first_name=?, last_name=?, initials=?, title =?" & _
                        ", address1=?, address2=?, address3=?, address4=?, address5=?, phone=? " & _
                        ", modify_date=? ,modify_time=?" & _
                        " where person.status = 0 And employee = ?"
        informixCmd = New Odbc.OdbcCommand(updSQL, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("pin", laxidPersonTableReader("pin").ToString)
        informixCmd.Parameters.AddWithValue("first_name", laxidPersonTableReader("fname").ToString)
        informixCmd.Parameters.AddWithValue("last_name", laxidPersonTableReader("lname").ToString)
        informixCmd.Parameters.AddWithValue("initials", laxidPersonTableReader("middle").ToString)
        informixCmd.Parameters.AddWithValue("title", DBNull.Value) ' TBD
        informixCmd.Parameters.AddWithValue("address1", laxidPersonTableReader("street").ToString)
        informixCmd.Parameters.AddWithValue("address2", laxidPersonTableReader("aptno").ToString)
        informixCmd.Parameters.AddWithValue("address3", laxidPersonTableReader("city").ToString)
        informixCmd.Parameters.AddWithValue("address4", laxidPersonTableReader("state").ToString)
        informixCmd.Parameters.AddWithValue("address5", laxidPersonTableReader("zip").ToString & laxidPersonTableReader("country").ToString)
        informixCmd.Parameters.AddWithValue("phone", laxidPersonTableReader("wphone"))
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        informixCmd.Parameters.AddWithValue("employee", laxidPersonTableReader("emp_id").ToString)
        Dim nRows As Integer = 0
        Try
            nRows = informixCmd.ExecuteNonQuery()
        Catch ex As Exception
            TraceLog(0, "Exception in updating person record for " & laxidPersonTableReader("emp_id").ToString & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If nRows > 0 Then ' person exists. update user data for this person
            InsertOrUpdateUserData(personUserDataMap, laxidPersonTableReader)
        End If
        Return nRows
    End Function
    Function InsertPersonRecord(ByVal laxidPersonTableReader As SqlDataReader, ByVal co_div As String) As Integer
        Dim dept_id As Integer = 0

        Dim informixCmd As Odbc.OdbcCommand
        If Not IsNothing(co_div) Then
            informixCmd = New Odbc.OdbcCommand("SELECT id from department where division=?", InformixReadConnection)
            informixCmd.Parameters.AddWithValue("division", co_div)
            Integer.TryParse(informixCmd.ExecuteScalar, dept_id)
            If dept_id = 0 Then
                TraceLog(1, String.Format("Department {0} Does not exist. Failed to create person {1} with this department", co_div, laxidPersonTableReader("emp_id")))
            End If
        End If
        Dim insertSQL As String = "INSERT INTO informix.person (" + _
           "pin, status, type, person_kp_resp, person_trace, person_trace_alarm, " + _
           "employee, department, first_name,last_name, initials, title, address1, address2, address3, address4,address5, " + _
           "phone, phone2, 	reissue_cnt, apb, reader, access_date,access_time, access_tz, " + _
           "active_date, active_time, active_context, deactive_date, deactive_time, deactive_context, force_download, " + _
            "facility, modify_date, modify_time) " & _
            "values (?,?,?,?,?,?,?, ?,?,?,?,null,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        informixCmd = New Odbc.OdbcCommand(insertSQL, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("pin", laxidPersonTableReader("pin"))
        informixCmd.Parameters.AddWithValue("status", 0)
        informixCmd.Parameters.AddWithValue("type", 1)
        informixCmd.Parameters.AddWithValue("person_kp_resp", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_trace", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_trace_alarm", DBNull.Value)
        informixCmd.Parameters.AddWithValue("employee", laxidPersonTableReader("emp_id"))
        informixCmd.Parameters.AddWithValue("department", IIf(dept_id = 0, DBNull.Value, dept_id))
        informixCmd.Parameters.AddWithValue("first_name", laxidPersonTableReader("fname"))
        informixCmd.Parameters.AddWithValue("last_name", laxidPersonTableReader("lname"))
        informixCmd.Parameters.AddWithValue("initials", laxidPersonTableReader("middle"))
        informixCmd.Parameters.AddWithValue("address1", laxidPersonTableReader("street").ToString)
        informixCmd.Parameters.AddWithValue("address2", laxidPersonTableReader("aptno").ToString)
        informixCmd.Parameters.AddWithValue("address3", laxidPersonTableReader("city").ToString)
        informixCmd.Parameters.AddWithValue("address4", laxidPersonTableReader("state").ToString)
        informixCmd.Parameters.AddWithValue("address5", laxidPersonTableReader("zip").ToString & laxidPersonTableReader("country").ToString)
        informixCmd.Parameters.AddWithValue("phone", laxidPersonTableReader("wphone"))
        informixCmd.Parameters.AddWithValue("phone2", DBNull.Value)
        informixCmd.Parameters.AddWithValue("reissue_cnt", DBNull.Value)
        informixCmd.Parameters.AddWithValue("apb", 0)
        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
        informixCmd.Parameters.AddWithValue("active_date", 19710101)
        informixCmd.Parameters.AddWithValue("active_time", 235959)
        informixCmd.Parameters.AddWithValue("active_context", 1)
        informixCmd.Parameters.AddWithValue("deactive_date", 20201231)
        informixCmd.Parameters.AddWithValue("deactive_time", 235959)
        informixCmd.Parameters.AddWithValue("deactive_context", 1)
        informixCmd.Parameters.AddWithValue("force_download", 0)
        informixCmd.Parameters.AddWithValue("facility", -1)
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        Dim nRows As Integer = 0
        Try
            nRows = informixCmd.ExecuteNonQuery()
            If (nRows = 0) Then
                TraceLog(1, "0 Rows Inserted for New Person = " & co_div + "." + laxidPersonTableReader("emp_id"))
            Else
                TraceLog(3, "New Person created " & co_div + "." + laxidPersonTableReader("emp_id"))
            End If
        Catch ex As Exception
            TraceLog(0, "Exception in inserting Person " & co_div + "." + laxidPersonTableReader("emp_id").ToString & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If nRows > 0 Then
            ' Create person_user records for data items known from person information
            InsertOrUpdateUserData(personUserDataMap, laxidPersonTableReader)
        End If
    End Function
    Sub TransferPerson(ByVal transactionID As Integer)
        ' Read record from Transfer_person table with matching transfer-id
        Dim refCmd = New SqlCommand("Select * from Transfer_Person where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidPersonTableReader = refCmd.ExecuteReader

        While laxidPersonTableReader.Read
            ' Update Person based on employee id alone. (note that this may update multiple records)
            Dim NRows As Integer = UpdatePersonRecords(laxidPersonTableReader)
            If (NRows > 0) Then ' Some records were updated. So this person already exists
                TraceLog(3, "Person ID =" & laxidPersonTableReader("emp_id").ToString & ". " & NRows & " Records Updated")
            Else
                ' No Records updated so Insert Person. It is a new person
                ' following line commented. Person record is created during badge record transfer
                'InsertPersonRecord(laxidPersonTableReader, Nothing)
            End If
        End While
        laxidPersonTableReader.Close()
    End Sub
    Sub InsertOrUpdateUserData(ByVal userDataMap As Hashtable, ByVal laxidPersonTableReader As SqlDataReader)
        Dim cmd As OdbcCommand
        Dim employee As String = laxidPersonTableReader("emp_id").ToString

        ' first readback person_id
        cmd = New OdbcCommand("Select id from person where employee=?", InformixReadConnection) ' will find only one record
        cmd.Parameters.AddWithValue("employee", employee)
        Dim person_id As Integer = 0
        Integer.TryParse(cmd.ExecuteScalar(), person_id)
        If person_id = 0 Then
            TraceLog(1, "Failed to retrieve Inserted Person " & employee)
            Return
        End If

        cmd.Dispose()
        For Each item As DictionaryEntry In userDataMap
            Dim slot_number As Integer = item.Value
            Dim user_data As String = laxidPersonTableReader(item.Key).ToString
            Try
                ' try to update first. if update does not change any records then insert
                cmd = New OdbcCommand("Update person_user set description=?, modify_date=?, modify_time=? WHERE person_id=? AND slot_number=?", InformixWriteConnection)
                cmd.Parameters.AddWithValue("description", laxidPersonTableReader(item.Key))
                cmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                cmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                cmd.Parameters.AddWithValue("person_id", person_id)
                cmd.Parameters.AddWithValue("slot_number", item.Value)
                If cmd.ExecuteNonQuery() <= 0 Then ' no rows were updated. Insert this slot
                    cmd.Dispose()
                    cmd = New OdbcCommand("Insert Into person_user (id, description, person_id, slot_number, facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?)", InformixWriteConnection)
                    cmd.Parameters.AddWithValue("id", person_id * 100 + item.Value)
                    cmd.Parameters.AddWithValue("description", laxidPersonTableReader(item.Key))
                    cmd.Parameters.AddWithValue("person_id", person_id)
                    cmd.Parameters.AddWithValue("slot_number", item.Value)
                    cmd.Parameters.AddWithValue("facility", -1)
                    cmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    cmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                End If
            Catch ex As Exception
                TraceLog(1, String.Format("Exception in setting user value employee={0}, slot={1}, value={2}:{3} ", employee, slot_number, user_data, ex.Message))
            End Try
        Next

    End Sub
    Function UpdateBadgeRecord(ByVal bid As String, ByVal laxidBadgeReader As SqlDataReader) As Integer
        Dim dept As String = "9" + laxidBadgeReader("CO_ID").ToString + laxidBadgeReader("Div_ID").ToString
        Dim person_id As Integer = ObtainPersonID(laxidBadgeReader("emp_id").ToString, dept)
        Dim desc As String = laxidBadgeReader("COLOR").ToString.Trim

        Dim sqlText As String = "update informix.badge set description=?, person_id=?, return_date= ?, return_time= ?,return_tz= ?" + _
                                ",status=?, modify_date= ?, modify_time= ? where bid = ?"
        Dim informixCmd As New Odbc.OdbcCommand(sqlText, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("description", desc)
        informixCmd.Parameters.AddWithValue("person_id", person_id)
        informixCmd.Parameters.AddWithValue("return_date", MakeIntDate(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidBadgeReader("RETURN_DT")), DBNull.Value, 342))
        informixCmd.Parameters.AddWithValue("status", Me.statusMapTable(laxidBadgeReader("status").ToString))
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        informixCmd.Parameters.AddWithValue("bid", bid)
        Dim NRows As Integer = 0
        Try
            NRows = informixCmd.ExecuteNonQuery()
            If (NRows = 0) Then
                TraceLog(1, "Failed to Update badge number= " & bid & ", Status=" & laxidBadgeReader("status"))
            Else
                TraceLog(3, "Updated badge number= " & bid & ", Status=" & laxidBadgeReader("status"))
            End If
        Catch ex As Exception
            TraceLog(0, "Exception updating badge number=" & bid & ":" & ex.Message)
        End Try
        informixCmd.Dispose()
        If NRows > 0 Then
            InsertOrUpdateUserData(badgeUserDataMap, laxidBadgeReader)
        End If
        Return NRows
    End Function

    Function InsertBadgeRecord(ByVal bid As String, ByVal laxidBadgeReader As SqlDataReader) As Integer
        Dim sqlText As String = ""
        ' Not an exitsting badge. Create new badge
        Dim dept As String = "9" + laxidBadgeReader("CO_ID").ToString + laxidBadgeReader("Div_ID").ToString
        Dim person_id As Integer = ObtainPersonID(laxidBadgeReader("emp_id").ToString, dept)
        Dim desc As String = laxidBadgeReader("COLOR").ToString.Trim

        Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
           "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
           "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
           "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
           "facility, modify_date, modify_time) " + _
            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        Dim informixCmd As New Odbc.OdbcCommand(cmdText, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("description", desc)
        informixCmd.Parameters.AddWithValue("bid", bid)
        informixCmd.Parameters.AddWithValue("status", Me.statusMapTable(laxidBadgeReader("status").ToString))
        informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
        informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_id", person_id)
        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
        informixCmd.Parameters.AddWithValue("issue_date", MakeIntDate(laxidBadgeReader("issue_dt")))
        informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(laxidBadgeReader("issue_dt")))
        informixCmd.Parameters.AddWithValue("issue_context", 1)
        informixCmd.Parameters.AddWithValue("expired_date", MakeIntDate(laxidBadgeReader("exp_dt")))
        informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(BadgeReader("expired_time")))
        informixCmd.Parameters.AddWithValue("expired_context", 1)
        informixCmd.Parameters.AddWithValue("return_date", MakeIntDate(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidBadgeReader("RETURN_DT")))
        Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidBadgeReader("RETURN_DT")), DBNull.Value, Pacific_time))
        informixCmd.Parameters.AddWithValue("usage_count", -1)
        informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
        informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
        informixCmd.Parameters.AddWithValue("bid_format_id", IIf(bid.StartsWith("00101"), 15, 19))
        informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
        informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
        informixCmd.Parameters.AddWithValue("unique_id", laxidBadgeReader("BadgeNo").ToString)
        informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
        informixCmd.Parameters.AddWithValue("facility", -1)
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        Dim NRows As Integer = 0
        Try
            NRows = informixCmd.ExecuteNonQuery()
            If (NRows = 0) Then
                TraceLog(1, "Failed to create new badge " & bid)
            Else
                TraceLog(3, "Created new badge " & bid)
            End If

        Catch ex As Exception
            TraceLog(0, "Exception in inserting badge " & bid & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If NRows > 0 Then
            InsertOrUpdateUserData(badgeUserDataMap, laxidBadgeReader)
        End If
        Return NRows
    End Function
    Sub TransferBadge(ByVal transactionID As Integer)
        ' Read record from Transfer_badge table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_Badge where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidBadgeReader As SqlDataReader = refCmd.ExecuteReader
        Dim informixCmd As Odbc.OdbcCommand

        While laxidBadgeReader.Read
            Dim bid As String = laxidBadgeReader("Badgeno").ToString.Trim

            'Mag Stripe
            Dim bidStr As String = "00101" + bid
            informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixReadConnection)
            '1. select all lines where employee= given employee and department= given department
            informixCmd.Parameters.AddWithValue("bid", bidStr)
            Dim informixReader As Odbc.OdbcDataReader
            informixReader = informixCmd.ExecuteReader
            Dim recordExists As Boolean = informixReader.Read
            informixCmd.Dispose()
            informixReader.Close()
            If recordExists Then
                UpdateBadgeRecord(bidStr, laxidBadgeReader)
            Else
                InsertBadgeRecord(bidStr, laxidBadgeReader)
            End If

            'iClass Number
            bid = laxidBadgeReader("Cardno").ToString.Trim
            If bid = "0" Then Continue While ' Badge is not activated at swipe station yet
            bidStr = "00520" + bid
            informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixReadConnection)
            '1. select all lines where employee= given employee and department= given department
            informixCmd.Parameters.AddWithValue("bid", bidStr)
            informixReader = informixCmd.ExecuteReader
            recordExists = informixReader.Read
            If recordExists Then
                UpdateBadgeRecord(bidStr, laxidBadgeReader)
            Else
                InsertBadgeRecord(bidStr, laxidBadgeReader)
            End If
        End While
        laxidBadgeReader.Close()
    End Sub

    Function GetPersonID(ByVal employee As String, ByVal division As String) As Integer
        Dim sqlText As String = "SELECT p.id id FROM Person p Inner Join Department d on p.department=d.id Where p.Employee=? And d.division=?"
        Dim informixCmd As New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        informixCmd.Parameters.AddWithValue("Employee", employee)
        informixCmd.Parameters.AddWithValue("Division", division)
        Dim person_id As Integer = informixCmd.ExecuteScalar
        informixCmd.Dispose()
        If Not IsNothing(person_id) Then If person_id > 0 Then Return person_id ' person already exists with given emp,dept
        Return 0
    End Function

    Function ObtainPersonID(ByVal employee As String, ByVal co_div As String) As Integer ' creates person if not exists
        ' Get person id if person already exists for given employee,dept
        ' Otherwise: creates person using earlier record of same employeeid and returns id
        Dim personID As Integer = GetPersonID(employee, co_div)
        If personID > 0 Then Return personID

        ' person does not exist in pp. Create the record
        Dim laxidConnectionForPersonRec = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
        Dim laxidPersonCmd As New SqlCommand("Select * from person where emp_id=@emp_id", laxidConnectionForPersonRec)
        laxidPersonCmd.Parameters.AddWithValue("emp_id", employee)
        Dim laxidPersonReader As SqlDataReader = laxidPersonCmd.ExecuteReader
        If Not laxidPersonReader.Read Then
            TraceLog(1, "No Person record for emp_id= " & employee)
            laxidPersonCmd.Dispose()
            laxidPersonReader.Close()
            Return 0
        End If
        InsertPersonRecord(laxidPersonReader, co_div)
        laxidPersonCmd.Dispose()
        laxidPersonReader.Close()
        Return GetPersonID(employee, co_div)
        '' Look for record with null department. This is unused person record (recently created but has no badges so far)
        'Dim informixReader As Odbc.OdbcDataReader
        'sqlText = "SELECT * FROM Person Where Employee=? And department is null"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        'informixCmd.Parameters.AddWithValue("Employee", employee)
        'informixCmd.Parameters.AddWithValue("Division", dept)
        'informixReader = informixCmd.ExecuteReader
        'If informixReader.Read Then ' Unused person record exists.
        '    ' Set department code in this person record and use it
        '    person_id = informixReader("id")
        '    sqlText = "UPDATE Person Set Department=(Select ID from Department Where division=?) Where ID=?"
        '    Dim updateCmd As New OdbcCommand(sqlText, InformixWriteConnection)
        '    updateCmd.Parameters.AddWithValue("division", dept)
        '    updateCmd.Parameters.AddWithValue("ID", person_id)
        '    If updateCmd.ExecuteNonQuery <= 0 Then
        '        TraceLog(1, "Failed to update Department in Person Record=" & person_id)
        '    Else
        '        Return person_id
        '    End If
        'End If
        '' Unused person record not found. Create new person record for this person
        '' first get department ID
        'sqlText = "SELECT id from department where division=?"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        'informixCmd.Parameters.AddWithValue("division", dept)
        'Dim dept_id As Integer = informixCmd.ExecuteScalar
        'If IsNothing(dept_id) Then
        '    TraceLog(1, String.Format("Department {0} Does not exist. Failed to create person {1} with this department", dept, employee))
        '    Return Nothing
        'End If

        'sqlText = " Insert into Person(pin,	status,	type, person_kp_resp,person_trace,person_trace_alarm,employee,department," & _
        '         "first_name,last_name,initials,title,address1,address2,address3,address4,address5,phone,phone2," & _
        '         "reissue_cnt,apb,reader,access_date,access_time,access_tz,active_date,active_time,active_context," & _
        '         "deactive_date,deactive_time,deactive_context,force_download,facility,modify_date,modify_time)" & _
        '         " SELECT First 1 pin,status,type,person_kp_resp,person_trace,person_trace_alarm,employee,?," & _
        '         "first_name,last_name,initials,title,address1,address2,address3,address4,address5,phone,phone2," & _
        '         "reissue_cnt,apb,reader,access_date,access_time,access_tz,active_date,active_time,active_context," & _
        '         "deactive_date,deactive_time,deactive_context,force_download,facility,modify_date,modify_time" & _
        '         "FROM person Where employee=?"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixWriteConnection)
        'informixCmd.Parameters.AddWithValue("department", dept_id)
        'informixCmd.Parameters.AddWithValue("employee", employee)
        'If informixCmd.ExecuteNonQuery <= 0 Then
        '    TraceLog(1, String.Format("Create Person (by copy) Failed. Employee {0}, Department {1}", employee, dept))
        '    informixCmd.Dispose()
        '    Return Nothing
        'End If

        'informixCmd.Dispose()
        'informixReader.Close()
        'Return ret
    End Function

    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
        If (GlobalErrorlevel >= level) Then
            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
            My.Computer.FileSystem.WriteAllText(logDir + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " L" & level & ": " & msg & vbCrLf, True)
        End If
    End Sub

    Function MakeIntTime(ByVal t As Object) As Object
        If DBNull.Value.Equals(t) Then Return DBNull.Value
        Dim dt As DateTime
        If Not DateTime.TryParse(t.ToString, dt) Then Return Nothing
        Return Integer.Parse(dt.ToString("HHmmss"))
    End Function

    Function MakeIntDate(ByVal t As Object) As Object
        If DBNull.Value.Equals(t) Then Return DBNull.Value
        Dim dt As DateTime
        If Not DateTime.TryParse(t.ToString, dt) Then Return Nothing
        Return Integer.Parse(dt.ToString("yyyyMMdd"))
    End Function
End Class



'Public Sub Main()
'    '

'    Dim refCmd As SqlCommand
'    Dim laxidControlReader As SqlDataReader = Nothing
'    'If Not IsNothing(Dts.Variables.Item("JobLogDir").Value) Then logDir = Dts.Variables.Item("JobLogDir").Value
'    Try
'        laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
'        laxidConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
'        picConnection = laxidManager.AcquireConnection(Nothing) ' Connection for reading picture from laxid
'        TraceLog(3, "Aquired connection to LaxBadgeSq")
'        'laxidConnection.Open()
'        informixManager = Dts.Connections("pp4test")
'        InformixConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
'        TraceLog(3, "Aquired connection to pp4test")
'    Catch ex As Exception
'        TraceLog(0, "Exception in connecting to the databases " & ex.ToString)
'        Return
'    End Try

'InformixConnection.Open()     ' when connected through AcquireConnection Open is not necessary
'Dim NumberOfRecordsToProcess As Integer = 200
'NumberOfRecordsToProcess = Dts.Variables("NumberOfrecords").Value
'Dim CmdTxt = "Select top" + NumberOfRecordsToProcess + "* from B2KCASI4 where transmit_dt is null"
'refCmd = New SqlCommand(CmdTxt, laxidConnection)
'    refCmd = New SqlCommand("Select  *  from Transfer_Control where End_transmit is null  ", laxidConnection)
''refCmd.Parameters.AddWithValue("NR", NumberOfRecordsToProcess)
'    Try
'        laxidControlReader = refCmd.ExecuteReader
'    Catch ex As Exception
'        TraceLog(0, "Exception in reading LAXID " + ex.ToString)
'    End Try

'    While laxidControlReader.Read
'        TraceLog(3, "============================Processing row with ID = " & laxidControlReader("ID"))
'Dim dtype As String
'Dim transNo As Integer
'        dtype = laxidControlReader("DataTypeUpdated")
'        transNo = laxidControlReader("ID")
'        Select Case dtype
'            Case "badge"
'' transfer badge info
'                TransferBadge(transNo)
'            Case "person"
''transfer person info 
'                TransferPerson(transNo)
'            Case "company"
''transfer company info
'                TransferCompany(transNo)
'            Case "category"
''transfer category info
'                TransferCategory(transNo)
'            Case Else
'' print message badly formed entry
'        End Select
'    End While
'    laxidConnection.Close()
'    InformixConnection.Close()
'    Dts.TaskResult = ScriptResults.Success
'End Sub
'    Sub TransferBadge(ByVal transactionNo As Integer)
'        Dim refCmd As SqlCommand
'        Dim BadgeReader As SqlDataReader = Nothing
'        Dim informixReader As Odbc.OdbcDataReader = Nothing
'        refCmd = New SqlCommand _
'        ("Select  *  from Transfer_Badge where transmission_date is null and transferID=? ", laxidConnection)
'        refCmd.Parameters.AddWithValue("transferID", transactionNo)
'        Try
'            BadgeReader = refCmd.ExecuteReader
'        Catch ex As Exception
'            TraceLog(0, "Exception in reading LAXID " + ex.ToString)
'        End Try

'        While BadgeReader.Read
'            Dim bid As String = BadgeReader("Badgeno")
'            Dim bidStr As String
'            bid = bid.Trim
'            If bid.Length < 7 Then
'                bidStr = "00520" + bid
'            Else
'                bidStr = "00101" + bid
'            End If
'            Dim informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixConnection)
'            '1. select all lines where employee= given employee and department= given department
'            informixCmd.Parameters.AddWithValue("bid", bidStr)
'            Try
'                informixReader = informixCmd.ExecuteReader
'            Catch ex As Exception
'                TraceLog(0, "Exception in reading Informix " + ex.ToString)
'            End Try

'            If informixReader.Read Then
'                Dim sqlText As String = "update informix.badge set return_date= ?, return_time= ?,return_tz= ?" + _
'                                        ",status=?,modify_date= ?,modify_time= ? where unique_id = ?"
'                informixCmd = New Odbc.OdbcCommand(sqlText, InformixConnection)
'                informixCmd.Parameters.AddWithValue("return_date", MakeInt(BadgeReader("RETURN_DATE")))
'                informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(BadgeReader("RETURN_TIME")))
'                informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(BadgeReader("RETURN_DATE")), DBNull.Value, 342))
'                informixCmd.Parameters.AddWithValue("status", BadgeReader("status"))
'                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
'                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
'                informixCmd.Parameters.AddWithValue("unique_id", bidStr)
'                Try
'                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
'                    If (NRows = 0) Then
'                        TraceLog(1, "Casi_Badge: warning: Failed to change status to " & BadgeReader("status") & " for badge number= " & bid)
'                    Else
'                        TraceLog(3, "Casi_Badge: Changed status to " & BadgeReader("status") & " for badge number= " & bid)
'                    End If

'                Catch ex As Exception
'                    TraceLog(0, "Casi_Badge:Exception updating badge  number =" & bid & ":" & ex.Message)
'                End Try

'                informixCmd.Dispose()
'                Return
'            End If

'            Dim desc As String = BadgeReader("first_name").ToString.Trim + " " + BadgeReader("last_name").ToString
'            If desc.Length() > 60 Then desc = desc.Substring(0, 60)

'            Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
'               "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
'               "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
'               "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
'               "facility, modify_date, modify_time) " + _
'                "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

'            informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
'            informixCmd.Parameters.AddWithValue("description", desc)
'            informixCmd.Parameters.AddWithValue("bid", bidStr)
'            informixCmd.Parameters.AddWithValue("status", BadgeReader("status"))
'            informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("person_id", personid)
'            informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("issue_date", MakeInt(BadgeReader("issue_date")))
'            informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(BadgeReader("issue_time")))
'            informixCmd.Parameters.AddWithValue("issue_context", 1)
'            informixCmd.Parameters.AddWithValue("expired_date", MakeInt(BadgeReader("expired_date")))
'            informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(BadgeReader("expired_time")))
'            informixCmd.Parameters.AddWithValue("expired_context", 1)
'            informixCmd.Parameters.AddWithValue("return_date", MakeInt(BadgeReader("RETURN_DATE")))
'            informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(BadgeReader("RETURN_TIME")))
'            Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
'            informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(BadgeReader("RETURN_DATE")), DBNull.Value, Pacific_time))
'            informixCmd.Parameters.AddWithValue("usage_count", -1)
'            informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("bid_format_id", MakeInt(BadgeReader("BID_FORMAT_ID")))
'            informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
'            informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
'            informixCmd.Parameters.AddWithValue("unique_id", BadgeReader("BID").ToString)
'            informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("facility", -1)
'            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
'            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
'            Try
'                Dim NRows As Integer = informixCmd.ExecuteNonQuery()
'                If (NRows = 0) Then
'                    TraceLog(1, "Casi_Badge: Warning: failed to create new badge. badge = " & bid)
'                Else
'                    TraceLog(3, "Casi_Badge: Created new badge.  badge = " & bid)
'                End If

'            Catch ex As Exception
'                TraceLog(0, "Casi_Badge:Exception in inserting badge = " & bid & ":" & ex.ToString)
'            End Try

'            informixCmd.Dispose()

'        End While
'    End Sub
'    Sub TransferPerson(ByVal transactionNo As Integer)
'    End Sub
'    Sub TransferCompany(ByVal transactionNo As Integer)
'    End Sub
'    Sub TransferCategory(ByVal transactionNo As Integer)
'    End Sub
'    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
'        If (GlobalErrorlevel >= level) Then
'            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
'            My.Computer.FileSystem.WriteAllText(logDir + fileName, "L" & level & ": " & msg & vbCrLf, True)
'        End If
'    End Sub
'    Function MakeIntTime(ByVal t As Object) As Integer
'        If DBNull.Value.Equals(t) Then Return Nothing
'        Return Integer.Parse(t.ToString.Replace(":", ""))
'    End Function
'    Function MakeInt(ByVal t As Object) As Integer
'        If DBNull.Value.Equals(t) Then Return Nothing
'        Return Integer.Parse(t.ToString)
'    End Function
'End Class
''--------------------------------------------------------------------------
''    Sub aaa()
''        Casi_Department(laxidReader)
''        While True
''            Dim personid As Integer = Casi_Person(laxidReader)
''            If IsNothing(personid) Then
''                ' Log the error that person record is not created
''                Continue While
''            End If
''            Update_person_user(laxidReader, personid)
''            Update_person_category(laxidReader, personid)
''            ' Casi_Picture(laxidReader, personid)
''            Casi_Badge(laxidReader, personid)
''            SetTransmitDate(laxidReader)
''        End While
''        laxidConnection.Close()
''        InformixConnection.Close()
''        'Console.WriteLine("End Time=" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'Console.WriteLine("Time Spent Dept=" & deptTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Person=" & personTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Category=" & catTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent User=" & userTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Badge=" & badgeTimer.Elapsed.ToString)
''        'Console.ReadKey()

''        Dts.TaskResult = ScriptResults.Success

''    End Sub
''    Sub Casi_Department(ByRef laxidReader As SqlDataReader)   ' in the original returns new department ID in case of error returns -101
''        Dim dept As String = "9" & laxidReader("Dept")
''        '   needs PP401.proteus.informix.department laxid.dbo.division laxid.dbo.company
''        '   laxid.dbo.contact
''        'assuming databases are open

''        '1. select all lines where depatment = given dept
''        Dim refCmd As Odbc.OdbcCommand
''        Dim reader As Odbc.OdbcDataReader = Nothing
''        refCmd = New Odbc.OdbcCommand("select id from informix.department where user1 = ?", InformixConnection)
''        refCmd.Parameters.AddWithValue("user1", dept)
''        '2. if found exit. 
''        Try
''            reader = refCmd.ExecuteReader
''            If reader.Read Then
''                '    in the original code if department is not found it is not 
''                '    This may have to be readdressed.
''                reader.Close()
''                refCmd.Dispose()
''                TraceLog(3, "Casi_Department: department already exists ID = " & reader("id"))
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading department " & ex.ToString)
''        End Try

''        '3. else  find max id in table
''        Dim maxId As Integer = 0
''        refCmd = New Odbc.OdbcCommand("select max(id) as maxId from informix.department", InformixConnection)
''        Try
''            reader = refCmd.ExecuteReader
''            If reader.Read() Then
''                If Not reader.IsDBNull(0) Then maxId = reader.GetInt32(reader.GetOrdinal("maxId"))
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading maxId " & ex.ToString)
''        End Try
''        refCmd.Dispose()
''        reader.Close()
''        '4 increment Id
''        maxId += 1

''        '5. create a row with this id and other data from joining tables
''        Dim cmdText As String = "select " + _
''                       "company.print_name,division.name," + _
''                       "contact.fname,contact.lname,division.phone as phone, division.co_id, division.id," + _
''                       "company.ind_type " + _
''                      "from division left outer join company on division.co_id = company.id " + _
''                      "left outer join contact on division.co_id = contact.co_id and contact.div_id = division.id " + _
''                      "where status ='active'  and contact_type = 'BADGE1' " + _
''                      "and division.co_id = substring('" & dept & "',2,4) and division.id = substring('" & dept & "',6,2)"

''        Dim laxidConnection2 = New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        laxidConnection2.Open()

''        Dim laxidCmd As New SqlCommand(cmdText, laxidConnection2)
''        laxidCmd.Parameters.AddWithValue("Dept", dept.Trim())
''        laxidCmd.Parameters.AddWithValue("Dept1", dept.Trim())
''        Dim laxidReaderDept As SqlDataReader = Nothing
''        Try
''            laxidReaderDept = laxidCmd.ExecuteReader
''            If Not laxidReaderDept.Read() Then
''                laxidCmd.Dispose()
''                laxidReaderDept.Close()
''                Return ' some error to be handled????????????????????????????????????????????????????????
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading select " & ex.ToString)
''        End Try



''        '6. insert row in the table
''        Dim insCmdText As String = "INSERT INTO informix.department (" + _
''                           "id,description,division,location,manager,phone,user1,user2," + _
''                   "facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?,?,?,?,?)"

''        Dim insCmd As New Odbc.OdbcCommand(insCmdText, InformixConnection)
''        insCmd.Parameters.AddWithValue("id", maxId)
''        Dim desc As String = dept + " " + laxidReaderDept("print_name").trim()
''        If desc.Length > 50 Then desc = desc.Substring(0, 50)
''        insCmd.Parameters.AddWithValue("description", desc)
''        insCmd.Parameters.AddWithValue("division", DBNull.Value)
''        Dim loc As String = laxidReaderDept("name")
''        If loc.Length > 60 Then loc.Substring(0, 60).Trim()
''        insCmd.Parameters.AddWithValue("location", loc)
''        insCmd.Parameters.AddWithValue("manager", laxidReaderDept("fname").Trim() + " " + laxidReaderDept("lname").Trim())
''        insCmd.Parameters.AddWithValue("phone", laxidReaderDept("phone"))
''        insCmd.Parameters.AddWithValue("user1", "9" + laxidReaderDept("co_id").Trim + laxidReaderDept("id").Trim)
''        insCmd.Parameters.AddWithValue("user2", laxidReaderDept("ind_type").Trim)
''        insCmd.Parameters.AddWithValue("facility", -1)
''        insCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        insCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim Nrows As Integer = insCmd.ExecuteNonQuery()
''            If (Nrows = 0) Then
''                TraceLog(1, "Casi_Department: Warning: Failed to create department 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim)
''            Else
''                TraceLog(3, "Casi_Department: New department created 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim & "was inserted successfully")
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department: Exception creating new department 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim & ":" & ex.ToString)
''        End Try

''    End Sub
''    Function Casi_Person(ByRef laxidReader As SqlDataReader) As Integer
''        Dim informixCmd = New Odbc.OdbcCommand("select id from informix.person where employee= ? and department= ?", InformixConnection)
''        '1. select all lines where employee= given employee and department= given department
''        informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''        informixCmd.Parameters.AddWithValue("department", laxidReader("dept"))
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Try
''            informixReader = informixCmd.ExecuteReader
''            '2. if found update
''            If informixReader.Read Then
''                Dim sqlText2 = "update informix.person set pin=? ,modify_date= ? ,modify_time= ? where person.status=0 and" + _
''                               " employee = ? and pin <> ?"
''                informixCmd = New Odbc.OdbcCommand(sqlText2, InformixConnection)
''                informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''                informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''                Try
''                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''                    If (NRows = 0) Then
''                        TraceLog(3, "Casi_Person: Person already exists Person ID =" & laxidReader("employee").ToString & "no change in pin number")
''                    Else
''                        TraceLog(3, "Casi_Person: Person already exists Person ID =" & laxidReader("employee").ToString & "pin number changed")
''                    End If
''                Catch ex As Exception
''                    TraceLog(0, "Casi_Person:Exception: failed to change pin for " & laxidReader("employee").ToString & ex.ToString)
''                End Try
''                ' update code                    not in the original ?????????????????????????????????
''                Dim idnumber As Integer = informixReader("id")
''                informixReader.Close()
''                informixCmd.Dispose()
''                Return idnumber
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in updating informix.person " & ex.ToString)
''        End Try

''        informixReader.Close()
''        informixCmd.Dispose()
''        '3. else  find max id in table
''        informixCmd = New Odbc.OdbcCommand("select max(id) as maxId from informix.person", InformixConnection)
''        Dim maxId As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader

''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in reading maxId " & ex.ToString)
''        End Try
''        '4 increment Id
''        maxId += 1
''        informixCmd.Dispose()
''        informixReader.Close()
''        '5. create a row with this id and other data from 
''        Dim cmdText As String = "INSERT INTO informix.person (" + _
''           "id,pin, status, type, person_kp_resp, person_trace, person_trace_alarm, " + _
''           "employee, department, first_name,last_name, initials, title, address1, address2, address3, address4,address5, " + _
''           "phone, phone2, 	reissue_cnt, apb, reader, access_date,access_time, access_tz, " + _
''           "active_date, active_time, active_context, deactive_date, deactive_time, deactive_context, force_download, " + _
''            "facility, modify_date, modify_time) " + _
''            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

''        informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
''        informixCmd.Parameters.AddWithValue("id", maxId)
''        informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''        informixCmd.Parameters.AddWithValue("status", 0)
''        informixCmd.Parameters.AddWithValue("type", 1)
''        informixCmd.Parameters.AddWithValue("person_kp_resp", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_trace", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_trace_alarm", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''        informixCmd.Parameters.AddWithValue("department", laxidReader("dept"))
''        informixCmd.Parameters.AddWithValue("first_name", laxidReader("first_name"))
''        informixCmd.Parameters.AddWithValue("last_name", laxidReader("last_name"))
''        informixCmd.Parameters.AddWithValue("initials", laxidReader("initials"))
''        informixCmd.Parameters.AddWithValue("title", laxidReader("user6"))
''        informixCmd.Parameters.AddWithValue("address1", laxidReader("address1"))
''        informixCmd.Parameters.AddWithValue("address2", laxidReader("address2"))
''        informixCmd.Parameters.AddWithValue("address3", laxidReader("address3"))
''        informixCmd.Parameters.AddWithValue("address4", laxidReader("address4"))
''        informixCmd.Parameters.AddWithValue("address5", laxidReader("address5"))
''        informixCmd.Parameters.AddWithValue("phone", laxidReader("phone"))
''        informixCmd.Parameters.AddWithValue("phone2", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("reissue_cnt", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("apb", 0)
''        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("active_date", 19710101)
''        informixCmd.Parameters.AddWithValue("active_time", 235959)
''        informixCmd.Parameters.AddWithValue("active_context", 1)
''        informixCmd.Parameters.AddWithValue("deactive_date", 20201231)
''        informixCmd.Parameters.AddWithValue("deactive_time", 235959)
''        informixCmd.Parameters.AddWithValue("deactive_context", 1)
''        informixCmd.Parameters.AddWithValue("force_download", 1)
''        informixCmd.Parameters.AddWithValue("facility", -1)
''        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Person:warning: failed to create New Person ID = " & laxidReader("employee"))
''            Else
''                TraceLog(3, "Casi_Person: New Person created successfully.  personID = " & laxidReader("employee"))
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in inserting Person ID for ID= " & laxidReader("employee").ToString & ":" & ex.ToString)
''            Return Nothing
''        End Try
''        informixCmd.Dispose()
''        Return maxId
''    End Function
''    Public Sub Casi_Picture(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        '
''        ' Check if image already exists for this person
''        Dim getPicCmd As New Odbc.OdbcCommand("SELECT * from images where person_id=?", InformixConnection)
''        getPicCmd.Parameters.AddWithValue("person_id", PersonId)
''        Dim getPicReader As Odbc.OdbcDataReader = Nothing
''        Try
''            getPicReader = getPicCmd.ExecuteReader()
''            If getPicReader.Read Then
''                ' Picture already exists for the person. (TBD: should it be overwritten?) for now do not update
''                getPicReader.Close()
''                getPicCmd.Dispose()
''                TraceLog(3, "Casi_Picture: Picture already exists")
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in Reading image from PP. " & ex.ToString)
''        End Try

''        Dim cmdFrom As New SqlCommand("SELECT Picture FROM Person WHERE EMP_ID=@EmployeeID", picConnection)
''        cmdFrom.Parameters.AddWithValue("EmployeeID", laxidReader("EMPLOYEE"))
''        Dim pictureReader As SqlDataReader = Nothing
''        Try
''            pictureReader = cmdFrom.ExecuteReader()
''            If Not pictureReader.Read Then
''                ' person record does not exitst in b2k. should not happen
''                pictureReader.Close()
''                cmdFrom.Dispose()
''                TraceLog(0, "Casi_Picture:Person Record not found in B2K for EMP_ID=" & laxidReader("EMPLOYEE"))
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in Reading person from B2K " & ex.ToString)
''        End Try


''        If pictureReader.IsDBNull(0) Then
''            ' picture is null in b2k
''            Return
''        End If

''        Dim insertCmd As New Odbc.OdbcCommand("INSERT INTO images (person_id, type, size, width, height, version, compression, image_data, creation_date, creation_time, modify_date, modify_time) " & _
''                                                " VALUES (?, 0, ?, 140, 160, '2.4.2',1, ?, ?,?,?,?)", InformixConnection)
''        Dim image() As Byte = pictureReader("Picture")
''        pictureReader.Close()
''        cmdFrom.Dispose()

''        image = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, System.Text.Encoding.Default, image)
''        insertCmd.Parameters.Clear()
''        insertCmd.Parameters.AddWithValue("person_id", PersonId)
''        insertCmd.Parameters.AddWithValue("size", image.Length)
''        insertCmd.Parameters.AddWithValue("image_data", image)
''        insertCmd.Parameters.AddWithValue("creation_date", Now.ToString("yyyyMMdd"))
''        insertCmd.Parameters.AddWithValue("creation_time", Now.ToString("HHmmss"))
''        insertCmd.Parameters.AddWithValue("modify_date", Now.ToString("yyyyMMdd"))
''        insertCmd.Parameters.AddWithValue("modify_time", Now.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = insertCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Picture: No pictures inserted informix.person")
''            Else
''                TraceLog(3, "Casi_Picture: Picture transferred to pp4 successfully. Person ID = " & laxidReader("EMPLOYEE"))
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in inserting Image for PersonID=" & PersonId & " : " & ex.Message)
''        End Try
''        insertCmd.Dispose()
''        '
''        Dts.TaskResult = ScriptResults.Success
''    End Sub

''    Sub Casi_Badge(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        ' TBD check for personID=nothing. log error that badge cannot be created

''        Dim bid As String = laxidReader("bid")
''        Dim bidStr As String
''        bid = bid.Trim
''        If bid.Length < 7 Then
''            bidStr = "00520" + bid
''        Else
''            bidStr = "00101" + bid
''        End If

''        Dim informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixConnection)
''        '1. select all lines where employee= given employee and department= given department
''        informixCmd.Parameters.AddWithValue("bid", bidStr)
''        Try
''            Dim informixReader As Odbc.OdbcDataReader = informixCmd.ExecuteReader
''            If informixReader.Read Then
''                Dim sqlText As String = "update informix.badge set return_date= ?, return_time= ?,return_tz= ?" + _
''                                        ",status=?,modify_date= ?,modify_time= ? where unique_id = ?"
''                informixCmd = New Odbc.OdbcCommand(sqlText, InformixConnection)
''                informixCmd.Parameters.AddWithValue("return_date", MakeInt(laxidReader("RETURN_DATE")))
''                informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidReader("RETURN_TIME")))
''                informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidReader("RETURN_DATE")), DBNull.Value, 342))
''                informixCmd.Parameters.AddWithValue("status", laxidReader("status"))
''                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                informixCmd.Parameters.AddWithValue("unique_id", bidStr)
''                Try
''                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''                    If (NRows = 0) Then
''                        TraceLog(1, "Casi_Badge: warning: Failed to change status to " & laxidReader("status") & " for badge number= " & bid)
''                    Else
''                        TraceLog(3, "Casi_Badge: Changed status to " & laxidReader("status") & " for badge number= " & bid)
''                    End If

''                Catch ex As Exception
''                    TraceLog(0, "Casi_Badge:Exception updating badge  number =" & bid & ":" & ex.Message)
''                End Try

''                informixCmd.Dispose()
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Badge:Exception in reading informix.badge " & ex.Message)
''        End Try

''        Dim desc As String = laxidReader("first_name").ToString.Trim + " " + laxidReader("last_name").ToString
''        If desc.Length() > 60 Then desc = desc.Substring(0, 60)

''        Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
''           "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
''           "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
''           "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
''           "facility, modify_date, modify_time) " + _
''            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

''        informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
''        informixCmd.Parameters.AddWithValue("description", desc)
''        informixCmd.Parameters.AddWithValue("bid", bidStr)
''        informixCmd.Parameters.AddWithValue("status", laxidReader("status"))
''        informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("issue_date", MakeInt(laxidReader("issue_date")))
''        informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(laxidReader("issue_time")))
''        informixCmd.Parameters.AddWithValue("issue_context", 1)
''        informixCmd.Parameters.AddWithValue("expired_date", MakeInt(laxidReader("expired_date")))
''        informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(laxidReader("expired_time")))
''        informixCmd.Parameters.AddWithValue("expired_context", 1)
''        informixCmd.Parameters.AddWithValue("return_date", MakeInt(laxidReader("RETURN_DATE")))
''        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidReader("RETURN_TIME")))
''        Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
''        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidReader("RETURN_DATE")), DBNull.Value, Pacific_time))
''        informixCmd.Parameters.AddWithValue("usage_count", -1)
''        informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("bid_format_id", MakeInt(laxidReader("BID_FORMAT_ID")))
''        informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
''        informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
''        informixCmd.Parameters.AddWithValue("unique_id", laxidReader("BID").ToString)
''        informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("facility", -1)
''        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Badge: Warning: failed to create new badge. badge = " & bid)
''            Else
''                TraceLog(3, "Casi_Badge: Created new badge.  badge = " & bid)
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Casi_Badge:Exception in inserting badge = " & bid & ":" & ex.ToString)
''        End Try

''        informixCmd.Dispose()
''    End Sub
''    Sub Update_person_user(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        'Dim stopWatch1 As New System.Diagnostics.Stopwatch
''        ''Console.WriteLine("======person_user before count========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        Dim informixCmd = New Odbc.OdbcCommand("select count(*) as userCount from informix.person_user where person_id = ?", InformixConnection)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Dim recCount As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then recCount = informixReader.GetDecimal(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_user:Exception failed to count the number of records for personID = " & PersonId & ":" & ex.ToString)
''        End Try

''        'Console.WriteLine("======person_user after count========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  count {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        informixReader.Close()
''        informixCmd.Dispose()
''        If recCount > 0 Then
''            TraceLog(1, "Update_person_user:Warning: person ID = " & PersonId.ToString & " already exists")
''            '  no upgrades are done ??????????????????????????????????????????????????
''            Return
''        End If
''        ' no user data found so fill it
''        ' 1. find largest id  in person-user
''        'Console.WriteLine("======person_user before maxid========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        informixCmd = New Odbc.OdbcCommand("select  max(id) from informix.person_user", InformixConnection)
''        Dim maxRowId As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxRowId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_user:Exception in reading maxId from informix.person_user " & ex.ToString)
''        End Try

''        'Console.WriteLine("======person_user after maxid========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  maxid {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        '' 3. loop i=1 to 40   read user-i 
''        ''     increment largest id
''        ''     If useri null continue 
''        ''     insert in slot number i
''        ''     fill the user-i 
''        ''    end loop  
''        ''Console.WriteLine("======person_user before insert loop========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        Dim NRows As Integer
''        For i = 1 To 40
''            If DBNull.Value.Equals(laxidReader("user" & i)) Then
''                Continue For
''            End If
''            maxRowId += 1
''            informixCmd = New Odbc.OdbcCommand("insert into informix.person_user(id,description,person_id," + _
''                                               "slot_number,facility,modify_date,modify_time) " + _
''                                               "values(?,?,?,?,?,?,?)", InformixConnection)
''            informixCmd.Parameters.AddWithValue("id", maxRowId)
''            informixCmd.Parameters.AddWithValue("description", laxidReader("user" & i.ToString))
''            informixCmd.Parameters.AddWithValue("person_id", PersonId)
''            informixCmd.Parameters.AddWithValue("slot_number", i)
''            informixCmd.Parameters.AddWithValue("facility", -1)
''            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''            Try
''                informixCmd.ExecuteNonQuery()
''                NRows = informixCmd.ExecuteNonQuery()
''                If (NRows = 0) Then
''                    TraceLog(1, "Update_person_user: warning: failed to insert slot " & i & " for personID = " & PersonId)
''                Else
''                    TraceLog(4, "Update_person_user: successfully inserted slot " & i & " for personID = " & PersonId)
''                End If

''            Catch ex As Exception
''                TraceLog(0, "Update_person_user:Exception  inserting  slot " & i & " for personID = " & PersonId & ":" & ex.ToString)
''            End Try

''        Next
''        '  Tr
''        ''Console.WriteLine("======person_user after insert loop========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  inserts {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        ' 4.
''    End Sub
''    Sub Update_person_category(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        ' no user data found so fill it
''        ' 1. find largest id  in person-user
''        Dim informixCmd = New Odbc.OdbcCommand("select  max(id) from informix.person_category", InformixConnection)
''        Dim maxRowId As Integer = 0
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxRowId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_category:Exception in reading maxId from informix.person_category" & ex.Message)
''        End Try

''        informixReader.Close()
''        informixCmd.Dispose()
''        '2. find all slots belonging to a person
''        informixCmd = New Odbc.OdbcCommand("select slot_number from informix.person_category where person_id = ? order by slot_number", InformixConnection)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        Try
''            informixReader = informixCmd.ExecuteReader
''        Catch ex As Exception
''            TraceLog(0, "Update_person_category:Exception in reading informix.person_category " & ex.Message)
''        End Try


''        Dim found As Boolean
''        Dim foundSlot As Integer
''        Dim slotNum As Integer
''        If informixReader.Read() Then
''            slotNum = informixReader("slot_number")
''        Else
''            slotNum = 9999
''        End If
''        Dim NRows As Integer
''        For i = 1 To 50
''            found = False
''            If (slotNum = i) Then
''                found = True
''                If informixReader.Read() Then
''                    slotNum = informixReader("slot_number")
''                Else
''                    slotNum = 9999
''                End If
''            End If

''            If (found) Then
''                '  not found in input therefore delete slot
''                If DBNull.Value.Equals(laxidReader("Category" & i.ToString)) Then
''                    informixCmd = New Odbc.OdbcCommand("delete from informix.person_category where person_id= ? and slot_number = ?", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("slot_number", foundSlot)
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category:warning: Failed to delete slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Deleted slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        End If
''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception in deleting slot_number " & foundSlot.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                Else
''                    ' found in informix therefore update slot
''                    informixCmd = New Odbc.OdbcCommand("update informix.person_category set category_id=?,modify_date=?,modify_time=? " + _
''                                 " where person_id= ? and slot_number=?", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("category_id", laxidReader("category" & i.ToString))
''                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("slot_number", i)
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category:warning: Failed to update slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Updated slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        End If
''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception in updating slot_number " & foundSlot.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                End If
''            Else ' not found in informix therefore insert slot
''                If Not DBNull.Value.Equals(laxidReader("Category" & i.ToString)) Then
''                    maxRowId += 1
''                    informixCmd = New Odbc.OdbcCommand("insert into informix.person_category(id,person_id," + _
''                                                   "category_id,slot_number,facility,modify_date,modify_time) " + _
''                                                   "values(?,?,?,?,?,?,?)", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("id", maxRowId)
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("category_id", laxidReader("category" & i.ToString))
''                    informixCmd.Parameters.AddWithValue("slot_number", i)
''                    informixCmd.Parameters.AddWithValue("facility", -1)
''                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category: warning: Failed to insert slot_number " & i.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Inserted slot_number " & i.ToString & " for person_id= " & PersonId)
''                        End If

''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception inserting slot_number " & i.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                End If
''            End If
''        Next

''        ' 4.
''    End Sub
''    Sub SetTransmitDate(ByRef laxidReader As SqlDataReader)

''        'Dim laxidConnectionUpdate As SqlConnection = New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        'laxidConnectionUpdate.Open()
''        laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
''        Dim laxidConnectionUpdate As SqlConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        Dim laxidCmd As SqlCommand
''        laxidCmd = New SqlCommand("Update B2KCASI4 set transmit_dt = @transmit_dt where id = @id", laxidConnectionUpdate)
''        laxidCmd.Parameters.AddWithValue("transmit_dt", DateTime.Now.ToString)
''        laxidCmd.Parameters.AddWithValue("id", laxidReader("ID"))
''        Try
''            Dim NRows As Integer = laxidCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "warning: failed to set transmission date for id =" & laxidReader("ID").ToString)
''            Else
''                TraceLog(3, "successfully setted  transmission date for id =" & laxidReader("ID").ToString)
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Exception in setting transmit_dt field in B2KCASI4 for id = " & laxidReader("ID").ToString & ":" & ex.ToString)
''        End Try
''        laxidCmd.Dispose()
''        laxidConnectionUpdate.Close()
''    End Sub
''    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
''        If (GlobalErrorlevel >= level) Then
''            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
''            My.Computer.FileSystem.WriteAllText(logDir + fileName, "L" & level & ": " & msg & vbCrLf, True)
''        End If
''    End Sub
''End Class]]></ProjectItem>
          <ProjectItem
            Name="My Project\AssemblyInfo.vb"
            Encoding="UTF8"><![CDATA[Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("ST_fee19305817346c48778eab4d49c9257.vbproj")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("ST_fee19305817346c48778eab4d49c9257.vbproj")> 
<Assembly: AssemblyCopyright("Copyright @ Microsoft 2010")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 

<Assembly: ComVisible(False)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("c20cc24e-8008-4f27-b8aa-920f991bada3")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> ]]></ProjectItem>
          <ProjectItem
            Name="\my project\settings.designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class Settings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As Settings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New Settings),Settings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As Settings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.ST_fee19305817346c48778eab4d49c9257.vbproj.Settings
            Get
                Return Global.ST_fee19305817346c48778eab4d49c9257.vbproj.Settings.Default
            End Get
        End Property
    End Module
End Namespace]]></ProjectItem>
          <ProjectItem
            Name="\st_fee19305817346c48778eab4d49c9257.vbproj"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="utf-16"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <!-- This section defines project-level properties.

       Configuration - Specifies whether the default configuration is Release or Debug.
       Platform - Specifies what CPU the output of this project can run on.
       OutputType - Must be "Library" for VSTA.
       NoStandardLibraries - Set to "false" for VSTA.
       RootNamespace - In C#, this specifies the namespace given to new files.
                       In Visual Basic, all objects are wrapped in this namespace at runtime.
       AssemblyName - Name of the output assembly.
  -->
  <PropertyGroup>
    <ProjectTypeGuids>{30D016F9-3734-4E33-A861-5E7D899E18F3};{F184B08F-C81C-45F6-A57F-5ABD9991F28F}</ProjectTypeGuids>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <RootNamespace>ST_fee19305817346c48778eab4d49c9257.vbproj</RootNamespace>
    <AssemblyName>ST_fee19305817346c48778eab4d49c9257.vbproj</AssemblyName>
    <StartupObject></StartupObject>
    <OptionExplicit>On</OptionExplicit>
    <OptionCompare>Binary</OptionCompare>
    <OptionStrict>Off</OptionStrict>
    <OptionInfer>On</OptionInfer>
    <ProjectGuid>{F995AD3D-4007-43BE-A6C4-845CDAF3D2E9}</ProjectGuid>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <TargetFrameworkProfile></TargetFrameworkProfile>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Debug" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>true</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>true</DebugSymbols>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Release" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>false</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>false</DebugSymbols>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section enables pre- and post-build steps. However,
       it is recommended that MSBuild tasks be used instead of these properties.
  -->
  <PropertyGroup>
    <PreBuildEvent></PreBuildEvent>
    <PostBuildEvent></PostBuildEvent>
  </PropertyGroup>
  <!-- This sections specifies references for the project. -->
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
    <Reference Include="Microsoft.SqlServer.ManagedDTS, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
    <Reference Include="Microsoft.SqlServer.ScriptTask, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
  </ItemGroup>
  <!-- Visual Basic supports Importing namespaces (equivalent to using statements in C#).-->
  <ItemGroup>
    <Import Include="Microsoft.VisualBasic" />
    <Import Include="System" />
    <Import Include="System.Collections" />
    <Import Include="System.Data" />
    <Import Include="System.Diagnostics" />
    <Import Include="System.Windows.Forms" />
  </ItemGroup>
  <!-- This section defines the user source files that are part of the
       project.

       Compile - Specifies a source file to compile.
       EmbeddedResource - Specifies a .resx file for embedded resources.
       None - Specifies a file that is not to be passed to the compiler (for instance,
              a text file or XML file).
       AppDesigner - Specifies the directory where the application properties files can
                     be found.
  -->
  <ItemGroup>
    <AppDesigner Include="My Project\" />
    <Compile Include="My Project\AssemblyInfo.vb">
      <SubType>Code</SubType>
    </Compile>
    <EmbeddedResource Include="My Project\Resources.resx">
      <Generator>VbMyResourcesResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.vb</LastGenOutput>
      <CustomToolNamespace>My.Resources</CustomToolNamespace>
    </EmbeddedResource>
    <Compile Include="My Project\Resources.Designer.vb">
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
      <DependentUpon>Resources.resx</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <None Include="My Project\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.vb</LastGenOutput>
    </None>
    <Compile Include="My Project\Settings.Designer.vb">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="ScriptMain.vb">
      <SubType>Code</SubType>
    </Compile>
    <!-- Include the default configuration information and metadata files for the add-in.
         These files are copied to the build output directory when the project is
         built, and the path to the configuration file is passed to add-in on the command
         line when debugging.
    -->
  </ItemGroup>
  <!-- Include the build rules for a VB project.-->
  <Import Project="$(MSBuildBinPath)\Microsoft.VisualBasic.targets" />
  <!-- This section defines VSTA properties that describe the host-changable project properties. -->
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID="{30D016F9-3734-4E33-A861-5E7D899E18F3}">
        <ProjectProperties HostName="VSTAHostName" HostPackage="{B3A685AA-7EAF-4BC6-9940-57959FA5AC07}" ApplicationType="usd" Language="vb" TemplatesPath="" DebugInfoExeName="" />
        <Host Name="SSIS_ScriptTask" />
        <ProjectClient>
          <HostIdentifier>SSIS_ST140</HostIdentifier>
        </ProjectClient>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
</Project>]]></ProjectItem>
          <ProjectItem
            Name="\scriptmain.vb"
            Encoding="UTF8"><![CDATA[' Microsoft SQL Server Integration Services Script Task
' Write scripts using Microsoft Visual Basic 2008.
' The ScriptMain is the entry point class of the script.

Imports System
Imports System.Data
Imports System.Math
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.SqlClient
Imports System.Data.Odbc

<Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute> _
<System.CLSCompliantAttribute(False)> _
Partial Public Class ScriptMain
    Inherits Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase

    Enum ScriptResults
        Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success
        Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
    End Enum

    Dim logDir As String = "F:\JobLogs\"    ' TBD parameterize this
    Dim statusMapStr As String = "ACTIVE=0,CONFISCATED=5,EXPIRED=5,INVALID=5,LOST=4,RECALL=5,RETURNED=6,UNCLAIMED=5,CANCELLED=6,STOLEN=4,9-30-02  non FP=5"
    Dim specialCategoriesStartSlot As Integer = 9999    ' mmg: set it to large number to avoid any retention of categories across badge updates
    Dim statusMapTable As Hashtable
    Dim personUserDataMap As New Hashtable
    Dim badgeUserDataMap As New Hashtable


    ' The execution engine calls this method when the task executes.
    ' To access the object model, use the Dts property. Connections, variables, events,
    ' and logging features are available as members of the Dts property as shown in the following examples.
    '
    ' To reference a variable, call Dts.Variables("MyCaseSensitiveVariableName").Value
    ' To post a log entry, call Dts.Log("This is my log text", 999, Nothing)
    ' To fire an event, call Dts.Events.FireInformation(99, "test", "hit the help message", "", 0, True)
    '
    ' To use the connections collection use something like the following:
    ' ConnectionManager cm = Dts.Connections.Add("OLEDB")
    ' cm.ConnectionString = "Data Source=localhost;Initial Catalog=AdventureWorks;Provider=SQLNCLI10;Integrated Security=SSPI;Auto Translate=False;"
    '
    ' Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
    ' 
    ' To open Help, press F1.

    ' Microsoft SQL Server Integration Services Script Task
    ' Write scripts using Microsoft Visual Basic 2008.
    ' The ScriptMain is the entry point class of the script.
    Dim GlobalErrorlevel As Integer = 3
    '  connection attributes
    Dim laxidManager As ConnectionManager
    Dim informixManager As ConnectionManager
    Dim laxidConnection As SqlConnection
    Dim picConnection As SqlConnection
    Dim InformixReadConnection As Odbc.OdbcConnection
    Dim InformixWriteConnection As Odbc.OdbcConnection

    '
    '     stop watch attributes
    'Dim deptTimer As New System.Diagnostics.Stopwatch
    'Dim personTimer As New System.Diagnostics.Stopwatch
    'Dim catTimer As New System.Diagnostics.Stopwatch
    'Dim userTimer As New System.Diagnostics.Stopwatch
    'Dim badgeTimer As New System.Diagnostics.Stopwatch
    '     
    '    counters
    Dim NumberOfDepartmentsAdded As Integer = 0
    Dim numberOfPersonsAdded As Integer = 0
    Dim numberOfBadgesAdded As Integer = 0
    Dim NumberOfPicturesAdded As Integer = 0
    Dim NumberOfPerson_UserAdded As Integer = 0
    Dim NumberOfPerson_Category As Integer = 0

    '    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    'Main routine:
    Public Sub Main()
        ' Initialize badge status map
        Me.statusMapTable = New Hashtable
        Dim spl() As String = Me.statusMapStr.Split(",")
        For Each s As String In spl
            Dim nv() As String = s.Split("=")
            Me.statusMapTable.Add(nv(0), nv(1))
        Next

        badgeUserDataMap.Add("co_name", 1)
        badgeUserDataMap.Add("div_name", 2)
        badgeUserDataMap.Add("color", 3)
        'badgeUserDataMap.Add("user4", 4)
        badgeUserDataMap.Add("customs", 5)
        badgeUserDataMap.Add("job_title", 6)
        'badgeUserDataMap.Add("work_loc", 7)
        badgeUserDataMap.Add("driver", 8)
        badgeUserDataMap.Add("law", 9)
        badgeUserDataMap.Add("gates", 10)
        badgeUserDataMap.Add("atct", 11)

        personUserDataMap.Add("dob", 12)
        personUserDataMap.Add("ssn", 13)
        personUserDataMap.Add("ht_ft", 14)
        personUserDataMap.Add("ht_in", 15)
        personUserDataMap.Add("weight", 16)
        personUserDataMap.Add("sex", 17)
        personUserDataMap.Add("eyes", 18)
        personUserDataMap.Add("hair", 19)
        personUserDataMap.Add("ethnic", 20)
        personUserDataMap.Add("dl_no", 21)
        personUserDataMap.Add("dl_state", 22)
        personUserDataMap.Add("dl_expdt", 23)

        badgeUserDataMap.Add("badgeno", 25)

        Dim refCmd As SqlCommand
        'Dim laxidControlTableReader As SqlDataReader = Nothing
        'If Not IsNothing(Dts.Variables.Item("JobLogDir").Value) Then logDir = Dts.Variables.Item("JobLogDir").Value
        Try
            laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
            laxidConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
            picConnection = laxidManager.AcquireConnection(Nothing) ' Connection for reading picture from laxid
            TraceLog(3, "Aquired connection to LaxBadgeSq")
            'laxidConnection.Open()
            informixManager = Dts.Connections("pp4test")
            InformixReadConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
            InformixWriteConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
            TraceLog(3, "Aquired connection to pp4test")
        Catch ex As Exception
            TraceLog(0, "Exception in connecting to the databases " & ex.ToString)
            Return
        End Try

        Dim c As Integer = 0
        While c < 5000
            c = c + 1
            'refCmd = New SqlCommand("Select TOP 1 * from Transfer_Control where ID=<x>", laxidConnection)
            refCmd = New SqlCommand("Select TOP 1 * from Transfer_Control where End_transmit is null ORDER BY ID ", laxidConnection)
            Dim laxidControlTableReader = refCmd.ExecuteReader()

            If laxidControlTableReader.Read Then
                'Read a record from transfer_control table
                Dim transactionID As Integer = laxidControlTableReader("ID")
                refCmd.Dispose()
                laxidControlTableReader.Close()
                TraceLog(3, "============================Processing row with ID = " & transactionID)
                refCmd = New SqlCommand("UPDATE Transfer_Control Set Start_Transmit=getDate() WHERE ID=@ID", laxidConnection)
                refCmd.Parameters.AddWithValue("ID", transactionID)
                If refCmd.ExecuteNonQuery() <= 0 Then TraceLog(0, "Failed to update start_transmit date for transactionID " & transactionID)
                TransferDivision(transactionID)
                TransferPerson(transactionID)
                TransferCategory(transactionID)
                TransferBadge(transactionID)
                TransferBadgeCategory(transactionID)
                refCmd = New SqlCommand("UPDATE Transfer_Control Set End_Transmit=getDate() WHERE ID=@ID", laxidConnection)
                refCmd.Parameters.AddWithValue("ID", transactionID)
                If refCmd.ExecuteNonQuery() <= 0 Then TraceLog(0, "Failed to update End_transmit date for transactionID " & transactionID)
                refCmd.Dispose()

            Else
                Exit While
            End If
        End While
        laxidConnection.Close()
        InformixReadConnection.Close()
        InformixWriteConnection.Close()
        Dts.TaskResult = ScriptResults.Success
    End Sub
    Sub TransferDivision(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_CompanyDivision where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidDivisionTableReader = refCmd.ExecuteReader

        While laxidDivisionTableReader.Read
            Dim dept As String = "9" + laxidDivisionTableReader("CO_ID").ToString + laxidDivisionTableReader("DIV_ID").ToString
            Dim informixCmd As New Odbc.OdbcCommand("select id from informix.department where division = ?", InformixReadConnection)
            Dim reader As Odbc.OdbcDataReader = Nothing
            informixCmd.Parameters.AddWithValue("division", dept)
            '2. if found exit. 
            Try
                reader = informixCmd.ExecuteReader
                If reader.Read Then
                    TraceLog(3, "Department already exists ID = " & reader("id"))
                    Dim updCmdText As String = "UPDATE informix.department " + _
                                       " SET description=?,location=?,manager=?,phone=?,user1=?,user2=?," + _
                               "facility=?, modify_date=?, modify_time=? WHERE division=?"

                    Dim updCmd As New Odbc.OdbcCommand(updCmdText, InformixWriteConnection)
                    Dim ddesc As String = dept + " " + laxidDivisionTableReader("co_name").trim()
                    If ddesc.Length > 50 Then ddesc = ddesc.Substring(0, 50)
                    updCmd.Parameters.AddWithValue("description", ddesc)
                    Dim dloc As String = laxidDivisionTableReader("CO_Name") + ", " + laxidDivisionTableReader("DIV_Name")
                    If dloc.Length > 60 Then dloc.Substring(0, 60).Trim()
                    updCmd.Parameters.AddWithValue("location", "")
                    updCmd.Parameters.AddWithValue("manager", "")
                    updCmd.Parameters.AddWithValue("phone", "")
                    updCmd.Parameters.AddWithValue("user1", laxidDivisionTableReader("CO_NAME").ToString.Trim)
                    updCmd.Parameters.AddWithValue("user2", laxidDivisionTableReader("DIV_NAME").ToString.Trim)
                    updCmd.Parameters.AddWithValue("facility", -1)
                    updCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    updCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    updCmd.Parameters.AddWithValue("division", dept)
                    Try
                        Dim Nrows As Integer = updCmd.ExecuteNonQuery()
                        If (Nrows = 0) Then
                            TraceLog(1, "Warning: No rows updated by Update department " & dept)
                        Else
                            TraceLog(3, "Department Updated " & dept)
                        End If
                    Catch ex As Exception
                        TraceLog(0, "Casi_Department: Exception updating new department " & dept & ":" & ex.ToString)
                    End Try
                    updCmd.Dispose()
                    informixCmd.Dispose()
                    reader.Close()
                    Continue While
                End If
            Catch ex As Exception
                TraceLog(0, "Casi_Department:Exception in Reading department " & ex.ToString)
            End Try
            informixCmd.Dispose()
            reader.Close()
            ' Create department record in PP
            Dim insCmdText As String = "INSERT INTO informix.department (" + _
                               "description,division,location,manager,phone,user1,user2," + _
                       "facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?,?,?,?)"

            Dim insCmd As New Odbc.OdbcCommand(insCmdText, InformixWriteConnection)
            Dim desc As String = dept + " " + laxidDivisionTableReader("co_name").trim()
            If desc.Length > 50 Then desc = desc.Substring(0, 50)
            insCmd.Parameters.AddWithValue("description", desc)
            insCmd.Parameters.AddWithValue("division", dept)
            Dim loc As String = laxidDivisionTableReader("CO_Name") + ", " + laxidDivisionTableReader("DIV_Name")
            If loc.Length > 60 Then loc.Substring(0, 60).Trim()
            insCmd.Parameters.AddWithValue("location", "")
            insCmd.Parameters.AddWithValue("manager", "")
            insCmd.Parameters.AddWithValue("phone", "")
            insCmd.Parameters.AddWithValue("user1", laxidDivisionTableReader("CO_NAME").ToString.Trim)
            insCmd.Parameters.AddWithValue("user2", laxidDivisionTableReader("DIV_NAME").ToString.Trim)
            insCmd.Parameters.AddWithValue("facility", -1)
            insCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
            insCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
            Try
                Dim Nrows As Integer = insCmd.ExecuteNonQuery()
                If (Nrows = 0) Then
                    TraceLog(1, "Warning: Failed to create department " & dept)
                Else
                    TraceLog(3, "New department created " & dept)
                End If
            Catch ex As Exception
                TraceLog(0, "Casi_Department: Exception creating new department " & dept & ":" & ex.ToString)
            End Try
            insCmd.Dispose()
        End While
        refCmd.Dispose()
        laxidDivisionTableReader.Close()
    End Sub
    Sub TransferCategory(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_Category where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidReader = refCmd.ExecuteReader
        While laxidReader.Read
            Dim informixCmd As New OdbcCommand("UPDATE Category SET description=?,modify_date=?,modify_time=? WHERE id=?", InformixWriteConnection)
            informixCmd.Parameters.AddWithValue("description", laxidReader("descrp"))
            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
            informixCmd.Parameters.AddWithValue("id", laxidReader("CatID"))
            Try
                If informixCmd.ExecuteNonQuery <= 0 Then ' no records updated. Insert record
                    informixCmd.Dispose()
                    informixCmd = New OdbcCommand("INSERT INTO Category (id,description,permission_grp,m2mr_type,facility,modify_date,modify_time)" & _
                                                       " VALUES (?,?,-1,0,?,?,?)", InformixWriteConnection)
                    informixCmd.Parameters.AddWithValue("id", laxidReader("CatID"))
                    informixCmd.Parameters.AddWithValue("description", laxidReader("descrp"))
                    informixCmd.Parameters.AddWithValue("facility", -1)
                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    informixCmd.ExecuteNonQuery()
                    TraceLog(3, "New category created " & laxidReader("CatId"))
                Else ' update was successful
                    TraceLog(3, "Category record updated. id=" & laxidReader("CatId"))
                End If
            Catch ex As Exception
                TraceLog(0, "Exception creating Category ID=" & laxidReader("CatId") & ":" & ex.ToString)
            End Try
            informixCmd.Dispose()
        End While
        refCmd.Dispose()
        laxidReader.Close()
    End Sub

    Sub UpdateInsertCategories(ByVal laxCats As ArrayList, ByVal emp_id As String, ByVal co_div As String)
        ' read existing categories for this person
        Dim sql As String = "SELECT pc.id ID, pc.category_id, pc.slot_number, p.id person_id" & _
                            " from person p" & _
                            " inner join department d on p.department=d.id" & _
                            " left join person_category pc on pc.person_id=p.id" & _
                            " Where p.employee=? and d.division=? Order by pc.category_id"

        Dim informixCmd As New Odbc.OdbcCommand(sql, InformixReadConnection)
        informixCmd.Parameters.AddWithValue("employee", emp_id)
        informixCmd.Parameters.AddWithValue("division", co_div)
        Dim informixReader As OdbcDataReader = informixCmd.ExecuteReader
        Dim currentCats As New Hashtable
        Dim person_id As Integer = 0
        While informixReader.Read
            person_id = informixReader("person_id")
            If informixReader.IsDBNull(informixReader.GetOrdinal("category_id")) Then Continue While ' This will happen if there are no categories
            If informixReader("slot_number") < specialCategoriesStartSlot Then ' not a special category. Retain special categories in PP.
                ' tbd: change this when categories are managed in b2k
                If Not laxCats.Contains(informixReader("category_id").ToString) Then ' category not in new list
                    ' delete this catetory for this person from pp
                    Dim delCommand As New OdbcCommand("DELETE From person_category where person_id=? and category_id=?", InformixWriteConnection)
                    delCommand.Parameters.AddWithValue("person_id", person_id)
                    delCommand.Parameters.AddWithValue("category_id", informixReader("category_id"))
                    delCommand.ExecuteNonQuery()
                    TraceLog(3, String.Format("Person Category {0} Deleted for person {1}", informixReader("category_id"), co_div + "." + emp_id))
                    delCommand.Dispose()
                    Continue While
                End If
            End If
            ' store the category and slot_number for reference during insertion
            If Not currentCats.ContainsKey(informixReader("category_id").ToString) Then currentCats.Add(informixReader("category_id").ToString, informixReader("slot_number").ToString)
        End While
        informixCmd.Dispose()
        informixReader.Close()
        Dim freeSlot As Integer = 0
        Dim personCatMaxID As Integer = 0 ' this will be used as ID during insertion. Should not be required if ID was auto-increment field
        ' insert new category in laxCats i.e. the ones not present in currentCats
        For Each cat As Integer In laxCats
            If Not currentCats.ContainsKey(cat.ToString) Then ' this is new category
                ' find a free slot for insertig category
                Do
                    freeSlot = freeSlot + 1
                Loop While currentCats.ContainsValue(freeSlot.ToString) ' slot number is in use

                If personCatMaxID = 0 Then  ' lazy initialization of max ID
                    informixCmd = New OdbcCommand("SELECT MAX(ID) MaxID from person_category", InformixReadConnection)
                    Integer.TryParse(informixCmd.ExecuteScalar().ToString, personCatMaxID)
                    informixCmd.Dispose()
                End If
                ' insert the new category in pp
                Dim insCommand As New OdbcCommand("INSERT Into person_category (id, person_id, category_id, slot_number, facility, modify_date, modify_time)" & _
                                                  " VALUES (?,?,?,?,?,?,?)", InformixWriteConnection)
                personCatMaxID = personCatMaxID + 1
                TraceLog(3, String.Format("Adding Person_Category id={0}, person_id={1}, slot_number={2}, category_id={3} ", personCatMaxID, person_id, freeSlot, cat))
                insCommand.Parameters.AddWithValue("id", personCatMaxID)
                insCommand.Parameters.AddWithValue("person_id", person_id)
                insCommand.Parameters.AddWithValue("category_id", cat)
                insCommand.Parameters.AddWithValue("slot_number", freeSlot)
                insCommand.Parameters.AddWithValue("facility", -1)
                insCommand.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                insCommand.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                Try
                    If insCommand.ExecuteNonQuery() <= 0 Then
                        TraceLog(1, String.Format("Failed to insert Category {0} for person {1} ", cat, co_div + "." + emp_id))
                    Else
                        TraceLog(3, String.Format("Person Category {0} Added to person {1}", cat, co_div + "." + emp_id))
                    End If
                Catch ex As Exception
                    TraceLog(1, String.Format("Exception Inserting Category {0} for person {1}: {2} ", cat, co_div + "." + emp_id, ex.Message))
                End Try
                insCommand.Dispose()
            End If
        Next
    End Sub
    Sub TransferBadgeCategory(ByVal transactionID As Integer)
        'Read record from Transfer_Division table with matching transfer_id
        Dim refCmd = New SqlCommand("Select distinct emp_id, co_div, category_id from Transfer_BadgeCategory where transferID = @transferID order by emp_id, co_div, category_id", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim emp_id As String = ""
        Dim co_div As String = ""
        Dim laxidReader = refCmd.ExecuteReader
        Dim laxCats As New ArrayList
        Dim moreRecords As Boolean = laxidReader.Read
        While moreRecords
            laxCats.Clear() ' new emp. start over
            emp_id = laxidReader("emp_id")
            co_div = "9" & laxidReader("co_div")
            While moreRecords
                If emp_id.Equals(laxidReader("emp_id").ToString) And co_div.Equals("9" & laxidReader("co_div").ToString) Then
                    laxCats.Add(laxidReader("Category_ID").ToString) ' collect the categories in an array
                    moreRecords = laxidReader.Read
                Else
                    Exit While
                End If
            End While
            UpdateInsertCategories(laxCats, emp_id, co_div)
        End While
        refCmd.Dispose()
        laxidReader.Close()
    End Sub
    Function UpdatePersonRecords(ByVal laxidPersonTableReader As SqlDataReader) As Integer
        ' Update Person based on employee id alone. (note that this may update multiple records)
        Dim informixCmd As Odbc.OdbcCommand
        Dim updSQL = "update informix.person set pin=?" & _
                        ", first_name=?, last_name=?, initials=?, title =?" & _
                        ", address1=?, address2=?, address3=?, address4=?, address5=?, phone=? " & _
                        ", modify_date=? ,modify_time=?" & _
                        " where person.status = 0 And employee = ?"
        informixCmd = New Odbc.OdbcCommand(updSQL, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("pin", laxidPersonTableReader("pin").ToString)
        informixCmd.Parameters.AddWithValue("first_name", laxidPersonTableReader("fname").ToString)
        informixCmd.Parameters.AddWithValue("last_name", laxidPersonTableReader("lname").ToString)
        informixCmd.Parameters.AddWithValue("initials", laxidPersonTableReader("middle").ToString)
        informixCmd.Parameters.AddWithValue("title", DBNull.Value) ' TBD
        informixCmd.Parameters.AddWithValue("address1", laxidPersonTableReader("street").ToString)
        informixCmd.Parameters.AddWithValue("address2", laxidPersonTableReader("aptno").ToString)
        informixCmd.Parameters.AddWithValue("address3", laxidPersonTableReader("city").ToString)
        informixCmd.Parameters.AddWithValue("address4", laxidPersonTableReader("state").ToString)
        informixCmd.Parameters.AddWithValue("address5", laxidPersonTableReader("zip").ToString & laxidPersonTableReader("country").ToString)
        informixCmd.Parameters.AddWithValue("phone", laxidPersonTableReader("wphone"))
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        informixCmd.Parameters.AddWithValue("employee", laxidPersonTableReader("emp_id").ToString)
        Dim nRows As Integer = 0
        Try
            nRows = informixCmd.ExecuteNonQuery()
        Catch ex As Exception
            TraceLog(0, "Exception in updating person record for " & laxidPersonTableReader("emp_id").ToString & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If nRows > 0 Then ' person exists. update user data for this person
            InsertOrUpdateUserData(personUserDataMap, laxidPersonTableReader)
        End If
        Return nRows
    End Function
    Function InsertPersonRecord(ByVal laxidPersonTableReader As SqlDataReader, ByVal co_div As String) As Integer
        Dim dept_id As Integer = 0

        Dim informixCmd As Odbc.OdbcCommand
        If Not IsNothing(co_div) Then
            informixCmd = New Odbc.OdbcCommand("SELECT id from department where division=?", InformixReadConnection)
            informixCmd.Parameters.AddWithValue("division", co_div)
            Integer.TryParse(informixCmd.ExecuteScalar, dept_id)
            If dept_id = 0 Then
                TraceLog(1, String.Format("Department {0} Does not exist. Failed to create person {1} with this department", co_div, laxidPersonTableReader("emp_id")))
            End If
        End If
        Dim insertSQL As String = "INSERT INTO informix.person (" + _
           "pin, status, type, person_kp_resp, person_trace, person_trace_alarm, " + _
           "employee, department, first_name,last_name, initials, title, address1, address2, address3, address4,address5, " + _
           "phone, phone2, 	reissue_cnt, apb, reader, access_date,access_time, access_tz, " + _
           "active_date, active_time, active_context, deactive_date, deactive_time, deactive_context, force_download, " + _
            "facility, modify_date, modify_time) " & _
            "values (?,?,?,?,?,?,?, ?,?,?,?,null,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        informixCmd = New Odbc.OdbcCommand(insertSQL, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("pin", laxidPersonTableReader("pin"))
        informixCmd.Parameters.AddWithValue("status", 0)
        informixCmd.Parameters.AddWithValue("type", 1)
        informixCmd.Parameters.AddWithValue("person_kp_resp", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_trace", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_trace_alarm", DBNull.Value)
        informixCmd.Parameters.AddWithValue("employee", laxidPersonTableReader("emp_id"))
        informixCmd.Parameters.AddWithValue("department", IIf(dept_id = 0, DBNull.Value, dept_id))
        informixCmd.Parameters.AddWithValue("first_name", laxidPersonTableReader("fname"))
        informixCmd.Parameters.AddWithValue("last_name", laxidPersonTableReader("lname"))
        informixCmd.Parameters.AddWithValue("initials", laxidPersonTableReader("middle"))
        informixCmd.Parameters.AddWithValue("address1", laxidPersonTableReader("street").ToString)
        informixCmd.Parameters.AddWithValue("address2", laxidPersonTableReader("aptno").ToString)
        informixCmd.Parameters.AddWithValue("address3", laxidPersonTableReader("city").ToString)
        informixCmd.Parameters.AddWithValue("address4", laxidPersonTableReader("state").ToString)
        informixCmd.Parameters.AddWithValue("address5", laxidPersonTableReader("zip").ToString & laxidPersonTableReader("country").ToString)
        informixCmd.Parameters.AddWithValue("phone", laxidPersonTableReader("wphone"))
        informixCmd.Parameters.AddWithValue("phone2", DBNull.Value)
        informixCmd.Parameters.AddWithValue("reissue_cnt", DBNull.Value)
        informixCmd.Parameters.AddWithValue("apb", 0)
        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
        informixCmd.Parameters.AddWithValue("active_date", 19710101)
        informixCmd.Parameters.AddWithValue("active_time", 235959)
        informixCmd.Parameters.AddWithValue("active_context", 1)
        informixCmd.Parameters.AddWithValue("deactive_date", 20201231)
        informixCmd.Parameters.AddWithValue("deactive_time", 235959)
        informixCmd.Parameters.AddWithValue("deactive_context", 1)
        informixCmd.Parameters.AddWithValue("force_download", 0)
        informixCmd.Parameters.AddWithValue("facility", -1)
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        Dim nRows As Integer = 0
        Try
            nRows = informixCmd.ExecuteNonQuery()
            If (nRows = 0) Then
                TraceLog(1, "0 Rows Inserted for New Person = " & co_div + "." + laxidPersonTableReader("emp_id"))
            Else
                TraceLog(3, "New Person created " & co_div + "." + laxidPersonTableReader("emp_id"))
            End If
        Catch ex As Exception
            TraceLog(0, "Exception in inserting Person " & co_div + "." + laxidPersonTableReader("emp_id").ToString & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If nRows > 0 Then
            ' Create person_user records for data items known from person information
            InsertOrUpdateUserData(personUserDataMap, laxidPersonTableReader)
        End If
    End Function
    Sub TransferPerson(ByVal transactionID As Integer)
        ' Read record from Transfer_person table with matching transfer-id
        Dim refCmd = New SqlCommand("Select * from Transfer_Person where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidPersonTableReader = refCmd.ExecuteReader

        While laxidPersonTableReader.Read
            ' Update Person based on employee id alone. (note that this may update multiple records)
            Dim NRows As Integer = UpdatePersonRecords(laxidPersonTableReader)
            If (NRows > 0) Then ' Some records were updated. So this person already exists
                TraceLog(3, "Person ID =" & laxidPersonTableReader("emp_id").ToString & ". " & NRows & " Records Updated")
            Else
                ' No Records updated so Insert Person. It is a new person
                ' following line commented. Person record is created during badge record transfer
                'InsertPersonRecord(laxidPersonTableReader, Nothing)
            End If
        End While
        laxidPersonTableReader.Close()
    End Sub
    Sub InsertOrUpdateUserData(ByVal userDataMap As Hashtable, ByVal laxidPersonTableReader As SqlDataReader)
        Dim cmd As OdbcCommand
        Dim employee As String = laxidPersonTableReader("emp_id").ToString

        ' first readback person_id
        cmd = New OdbcCommand("Select id from person where employee=?", InformixReadConnection) ' will find only one record
        cmd.Parameters.AddWithValue("employee", employee)
        Dim person_id As Integer = 0
        Integer.TryParse(cmd.ExecuteScalar(), person_id)
        If person_id = 0 Then
            TraceLog(1, "Failed to retrieve Inserted Person " & employee)
            Return
        End If

        cmd.Dispose()
        For Each item As DictionaryEntry In userDataMap
            Dim slot_number As Integer = item.Value
            Dim user_data As String = laxidPersonTableReader(item.Key).ToString
            Try
                ' try to update first. if update does not change any records then insert
                cmd = New OdbcCommand("Update person_user set description=?, modify_date=?, modify_time=? WHERE person_id=? AND slot_number=?", InformixWriteConnection)
                cmd.Parameters.AddWithValue("description", laxidPersonTableReader(item.Key))
                cmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                cmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                cmd.Parameters.AddWithValue("person_id", person_id)
                cmd.Parameters.AddWithValue("slot_number", item.Value)
                If cmd.ExecuteNonQuery() <= 0 Then ' no rows were updated. Insert this slot
                    cmd.Dispose()
                    cmd = New OdbcCommand("Insert Into person_user (id, description, person_id, slot_number, facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?)", InformixWriteConnection)
                    cmd.Parameters.AddWithValue("id", person_id * 100 + item.Value)
                    cmd.Parameters.AddWithValue("description", laxidPersonTableReader(item.Key))
                    cmd.Parameters.AddWithValue("person_id", person_id)
                    cmd.Parameters.AddWithValue("slot_number", item.Value)
                    cmd.Parameters.AddWithValue("facility", -1)
                    cmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
                    cmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                End If
            Catch ex As Exception
                TraceLog(1, String.Format("Exception in setting user value employee={0}, slot={1}, value={2}:{3} ", employee, slot_number, user_data, ex.Message))
            End Try
        Next

    End Sub
    Function UpdateBadgeRecord(ByVal bid As String, ByVal laxidBadgeReader As SqlDataReader) As Integer
        Dim dept As String = "9" + laxidBadgeReader("CO_ID").ToString + laxidBadgeReader("Div_ID").ToString
        Dim person_id As Integer = ObtainPersonID(laxidBadgeReader("emp_id").ToString, dept)
        Dim desc As String = laxidBadgeReader("COLOR").ToString.Trim

        Dim sqlText As String = "update informix.badge set description=?, person_id=?, return_date= ?, return_time= ?,return_tz= ?" + _
                                ",status=?, modify_date= ?, modify_time= ? where bid = ?"
        Dim informixCmd As New Odbc.OdbcCommand(sqlText, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("description", desc)
        informixCmd.Parameters.AddWithValue("person_id", person_id)
        informixCmd.Parameters.AddWithValue("return_date", MakeIntDate(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidBadgeReader("RETURN_DT")), DBNull.Value, 342))
        informixCmd.Parameters.AddWithValue("status", Me.statusMapTable(laxidBadgeReader("status").ToString))
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        informixCmd.Parameters.AddWithValue("bid", bid)
        Dim NRows As Integer = 0
        Try
            NRows = informixCmd.ExecuteNonQuery()
            If (NRows = 0) Then
                TraceLog(1, "Failed to Update badge number= " & bid & ", Status=" & laxidBadgeReader("status"))
            Else
                TraceLog(3, "Updated badge number= " & bid & ", Status=" & laxidBadgeReader("status"))
            End If
        Catch ex As Exception
            TraceLog(0, "Exception updating badge number=" & bid & ":" & ex.Message)
        End Try
        informixCmd.Dispose()
        If NRows > 0 Then
            InsertOrUpdateUserData(badgeUserDataMap, laxidBadgeReader)
        End If
        Return NRows
    End Function

    Function InsertBadgeRecord(ByVal bid As String, ByVal laxidBadgeReader As SqlDataReader) As Integer
        Dim sqlText As String = ""
        ' Not an exitsting badge. Create new badge
        Dim dept As String = "9" + laxidBadgeReader("CO_ID").ToString + laxidBadgeReader("Div_ID").ToString
        Dim person_id As Integer = ObtainPersonID(laxidBadgeReader("emp_id").ToString, dept)
        Dim desc As String = laxidBadgeReader("COLOR").ToString.Trim

        Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
           "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
           "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
           "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
           "facility, modify_date, modify_time) " + _
            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        Dim informixCmd As New Odbc.OdbcCommand(cmdText, InformixWriteConnection)
        informixCmd.Parameters.AddWithValue("description", desc)
        informixCmd.Parameters.AddWithValue("bid", bid)
        informixCmd.Parameters.AddWithValue("status", Me.statusMapTable(laxidBadgeReader("status").ToString))
        informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
        informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
        informixCmd.Parameters.AddWithValue("person_id", person_id)
        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
        informixCmd.Parameters.AddWithValue("issue_date", MakeIntDate(laxidBadgeReader("issue_dt")))
        informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(laxidBadgeReader("issue_dt")))
        informixCmd.Parameters.AddWithValue("issue_context", 1)
        informixCmd.Parameters.AddWithValue("expired_date", MakeIntDate(laxidBadgeReader("exp_dt")))
        informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(BadgeReader("expired_time")))
        informixCmd.Parameters.AddWithValue("expired_context", 1)
        informixCmd.Parameters.AddWithValue("return_date", MakeIntDate(laxidBadgeReader("RETURN_DT")))
        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidBadgeReader("RETURN_DT")))
        Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidBadgeReader("RETURN_DT")), DBNull.Value, Pacific_time))
        informixCmd.Parameters.AddWithValue("usage_count", -1)
        informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
        informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
        informixCmd.Parameters.AddWithValue("bid_format_id", IIf(bid.StartsWith("00101"), 15, 19))
        informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
        informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
        informixCmd.Parameters.AddWithValue("unique_id", laxidBadgeReader("BadgeNo").ToString)
        informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
        informixCmd.Parameters.AddWithValue("facility", -1)
        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
        Dim NRows As Integer = 0
        Try
            NRows = informixCmd.ExecuteNonQuery()
            If (NRows = 0) Then
                TraceLog(1, "Failed to create new badge " & bid)
            Else
                TraceLog(3, "Created new badge " & bid)
            End If

        Catch ex As Exception
            TraceLog(0, "Exception in inserting badge " & bid & ":" & ex.ToString)
        End Try
        informixCmd.Dispose()
        If NRows > 0 Then
            InsertOrUpdateUserData(badgeUserDataMap, laxidBadgeReader)
        End If
        Return NRows
    End Function
    Sub TransferBadge(ByVal transactionID As Integer)
        ' Read record from Transfer_badge table with matching transfer_id
        Dim refCmd = New SqlCommand("Select * from Transfer_Badge where transferID = @transferID", laxidConnection)
        refCmd.Parameters.AddWithValue("transferID", transactionID)
        Dim laxidBadgeReader As SqlDataReader = refCmd.ExecuteReader
        Dim informixCmd As Odbc.OdbcCommand

        While laxidBadgeReader.Read
            Dim bid As String = laxidBadgeReader("Badgeno").ToString.Trim

            'Mag Stripe
            Dim bidStr As String = "00101" + bid
            informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixReadConnection)
            '1. select all lines where employee= given employee and department= given department
            informixCmd.Parameters.AddWithValue("bid", bidStr)
            Dim informixReader As Odbc.OdbcDataReader
            informixReader = informixCmd.ExecuteReader
            Dim recordExists As Boolean = informixReader.Read
            informixCmd.Dispose()
            informixReader.Close()
            If recordExists Then
                UpdateBadgeRecord(bidStr, laxidBadgeReader)
            Else
                InsertBadgeRecord(bidStr, laxidBadgeReader)
            End If

            'iClass Number
            bid = laxidBadgeReader("Cardno").ToString.Trim
            If bid = "0" Then Continue While ' Badge is not activated at swipe station yet
            bidStr = "00520" + bid
            informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixReadConnection)
            '1. select all lines where employee= given employee and department= given department
            informixCmd.Parameters.AddWithValue("bid", bidStr)
            informixReader = informixCmd.ExecuteReader
            recordExists = informixReader.Read
            If recordExists Then
                UpdateBadgeRecord(bidStr, laxidBadgeReader)
            Else
                InsertBadgeRecord(bidStr, laxidBadgeReader)
            End If
        End While
        laxidBadgeReader.Close()
    End Sub

    Function GetPersonID(ByVal employee As String, ByVal division As String) As Integer
        Dim sqlText As String = "SELECT p.id id FROM Person p Inner Join Department d on p.department=d.id Where p.Employee=? And d.division=?"
        Dim informixCmd As New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        informixCmd.Parameters.AddWithValue("Employee", employee)
        informixCmd.Parameters.AddWithValue("Division", division)
        Dim person_id As Integer = informixCmd.ExecuteScalar
        informixCmd.Dispose()
        If Not IsNothing(person_id) Then If person_id > 0 Then Return person_id ' person already exists with given emp,dept
        Return 0
    End Function

    Function ObtainPersonID(ByVal employee As String, ByVal co_div As String) As Integer ' creates person if not exists
        ' Get person id if person already exists for given employee,dept
        ' Otherwise: creates person using earlier record of same employeeid and returns id
        Dim personID As Integer = GetPersonID(employee, co_div)
        If personID > 0 Then Return personID

        ' person does not exist in pp. Create the record
        Dim laxidConnectionForPersonRec = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
        Dim laxidPersonCmd As New SqlCommand("Select * from person where emp_id=@emp_id", laxidConnectionForPersonRec)
        laxidPersonCmd.Parameters.AddWithValue("emp_id", employee)
        Dim laxidPersonReader As SqlDataReader = laxidPersonCmd.ExecuteReader
        If Not laxidPersonReader.Read Then
            TraceLog(1, "No Person record for emp_id= " & employee)
            laxidPersonCmd.Dispose()
            laxidPersonReader.Close()
            Return 0
        End If
        InsertPersonRecord(laxidPersonReader, co_div)
        laxidPersonCmd.Dispose()
        laxidPersonReader.Close()
        Return GetPersonID(employee, co_div)
        '' Look for record with null department. This is unused person record (recently created but has no badges so far)
        'Dim informixReader As Odbc.OdbcDataReader
        'sqlText = "SELECT * FROM Person Where Employee=? And department is null"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        'informixCmd.Parameters.AddWithValue("Employee", employee)
        'informixCmd.Parameters.AddWithValue("Division", dept)
        'informixReader = informixCmd.ExecuteReader
        'If informixReader.Read Then ' Unused person record exists.
        '    ' Set department code in this person record and use it
        '    person_id = informixReader("id")
        '    sqlText = "UPDATE Person Set Department=(Select ID from Department Where division=?) Where ID=?"
        '    Dim updateCmd As New OdbcCommand(sqlText, InformixWriteConnection)
        '    updateCmd.Parameters.AddWithValue("division", dept)
        '    updateCmd.Parameters.AddWithValue("ID", person_id)
        '    If updateCmd.ExecuteNonQuery <= 0 Then
        '        TraceLog(1, "Failed to update Department in Person Record=" & person_id)
        '    Else
        '        Return person_id
        '    End If
        'End If
        '' Unused person record not found. Create new person record for this person
        '' first get department ID
        'sqlText = "SELECT id from department where division=?"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixReadConnection)
        'informixCmd.Parameters.AddWithValue("division", dept)
        'Dim dept_id As Integer = informixCmd.ExecuteScalar
        'If IsNothing(dept_id) Then
        '    TraceLog(1, String.Format("Department {0} Does not exist. Failed to create person {1} with this department", dept, employee))
        '    Return Nothing
        'End If

        'sqlText = " Insert into Person(pin,	status,	type, person_kp_resp,person_trace,person_trace_alarm,employee,department," & _
        '         "first_name,last_name,initials,title,address1,address2,address3,address4,address5,phone,phone2," & _
        '         "reissue_cnt,apb,reader,access_date,access_time,access_tz,active_date,active_time,active_context," & _
        '         "deactive_date,deactive_time,deactive_context,force_download,facility,modify_date,modify_time)" & _
        '         " SELECT First 1 pin,status,type,person_kp_resp,person_trace,person_trace_alarm,employee,?," & _
        '         "first_name,last_name,initials,title,address1,address2,address3,address4,address5,phone,phone2," & _
        '         "reissue_cnt,apb,reader,access_date,access_time,access_tz,active_date,active_time,active_context," & _
        '         "deactive_date,deactive_time,deactive_context,force_download,facility,modify_date,modify_time" & _
        '         "FROM person Where employee=?"
        'informixCmd = New Odbc.OdbcCommand(sqlText, InformixWriteConnection)
        'informixCmd.Parameters.AddWithValue("department", dept_id)
        'informixCmd.Parameters.AddWithValue("employee", employee)
        'If informixCmd.ExecuteNonQuery <= 0 Then
        '    TraceLog(1, String.Format("Create Person (by copy) Failed. Employee {0}, Department {1}", employee, dept))
        '    informixCmd.Dispose()
        '    Return Nothing
        'End If

        'informixCmd.Dispose()
        'informixReader.Close()
        'Return ret
    End Function

    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
        If (GlobalErrorlevel >= level) Then
            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
            My.Computer.FileSystem.WriteAllText(logDir + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " L" & level & ": " & msg & vbCrLf, True)
        End If
    End Sub

    Function MakeIntTime(ByVal t As Object) As Object
        If DBNull.Value.Equals(t) Then Return DBNull.Value
        Dim dt As DateTime
        If Not DateTime.TryParse(t.ToString, dt) Then Return Nothing
        Return Integer.Parse(dt.ToString("HHmmss"))
    End Function

    Function MakeIntDate(ByVal t As Object) As Object
        If DBNull.Value.Equals(t) Then Return DBNull.Value
        Dim dt As DateTime
        If Not DateTime.TryParse(t.ToString, dt) Then Return Nothing
        Return Integer.Parse(dt.ToString("yyyyMMdd"))
    End Function
End Class



'Public Sub Main()
'    '

'    Dim refCmd As SqlCommand
'    Dim laxidControlReader As SqlDataReader = Nothing
'    'If Not IsNothing(Dts.Variables.Item("JobLogDir").Value) Then logDir = Dts.Variables.Item("JobLogDir").Value
'    Try
'        laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
'        laxidConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
'        picConnection = laxidManager.AcquireConnection(Nothing) ' Connection for reading picture from laxid
'        TraceLog(3, "Aquired connection to LaxBadgeSq")
'        'laxidConnection.Open()
'        informixManager = Dts.Connections("pp4test")
'        InformixConnection = informixManager.AcquireConnection(Nothing) 'New Odbc.OdbcConnection("Dsn=pphost")
'        TraceLog(3, "Aquired connection to pp4test")
'    Catch ex As Exception
'        TraceLog(0, "Exception in connecting to the databases " & ex.ToString)
'        Return
'    End Try

'InformixConnection.Open()     ' when connected through AcquireConnection Open is not necessary
'Dim NumberOfRecordsToProcess As Integer = 200
'NumberOfRecordsToProcess = Dts.Variables("NumberOfrecords").Value
'Dim CmdTxt = "Select top" + NumberOfRecordsToProcess + "* from B2KCASI4 where transmit_dt is null"
'refCmd = New SqlCommand(CmdTxt, laxidConnection)
'    refCmd = New SqlCommand("Select  *  from Transfer_Control where End_transmit is null  ", laxidConnection)
''refCmd.Parameters.AddWithValue("NR", NumberOfRecordsToProcess)
'    Try
'        laxidControlReader = refCmd.ExecuteReader
'    Catch ex As Exception
'        TraceLog(0, "Exception in reading LAXID " + ex.ToString)
'    End Try

'    While laxidControlReader.Read
'        TraceLog(3, "============================Processing row with ID = " & laxidControlReader("ID"))
'Dim dtype As String
'Dim transNo As Integer
'        dtype = laxidControlReader("DataTypeUpdated")
'        transNo = laxidControlReader("ID")
'        Select Case dtype
'            Case "badge"
'' transfer badge info
'                TransferBadge(transNo)
'            Case "person"
''transfer person info 
'                TransferPerson(transNo)
'            Case "company"
''transfer company info
'                TransferCompany(transNo)
'            Case "category"
''transfer category info
'                TransferCategory(transNo)
'            Case Else
'' print message badly formed entry
'        End Select
'    End While
'    laxidConnection.Close()
'    InformixConnection.Close()
'    Dts.TaskResult = ScriptResults.Success
'End Sub
'    Sub TransferBadge(ByVal transactionNo As Integer)
'        Dim refCmd As SqlCommand
'        Dim BadgeReader As SqlDataReader = Nothing
'        Dim informixReader As Odbc.OdbcDataReader = Nothing
'        refCmd = New SqlCommand _
'        ("Select  *  from Transfer_Badge where transmission_date is null and transferID=? ", laxidConnection)
'        refCmd.Parameters.AddWithValue("transferID", transactionNo)
'        Try
'            BadgeReader = refCmd.ExecuteReader
'        Catch ex As Exception
'            TraceLog(0, "Exception in reading LAXID " + ex.ToString)
'        End Try

'        While BadgeReader.Read
'            Dim bid As String = BadgeReader("Badgeno")
'            Dim bidStr As String
'            bid = bid.Trim
'            If bid.Length < 7 Then
'                bidStr = "00520" + bid
'            Else
'                bidStr = "00101" + bid
'            End If
'            Dim informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixConnection)
'            '1. select all lines where employee= given employee and department= given department
'            informixCmd.Parameters.AddWithValue("bid", bidStr)
'            Try
'                informixReader = informixCmd.ExecuteReader
'            Catch ex As Exception
'                TraceLog(0, "Exception in reading Informix " + ex.ToString)
'            End Try

'            If informixReader.Read Then
'                Dim sqlText As String = "update informix.badge set return_date= ?, return_time= ?,return_tz= ?" + _
'                                        ",status=?,modify_date= ?,modify_time= ? where unique_id = ?"
'                informixCmd = New Odbc.OdbcCommand(sqlText, InformixConnection)
'                informixCmd.Parameters.AddWithValue("return_date", MakeInt(BadgeReader("RETURN_DATE")))
'                informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(BadgeReader("RETURN_TIME")))
'                informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(BadgeReader("RETURN_DATE")), DBNull.Value, 342))
'                informixCmd.Parameters.AddWithValue("status", BadgeReader("status"))
'                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
'                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
'                informixCmd.Parameters.AddWithValue("unique_id", bidStr)
'                Try
'                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
'                    If (NRows = 0) Then
'                        TraceLog(1, "Casi_Badge: warning: Failed to change status to " & BadgeReader("status") & " for badge number= " & bid)
'                    Else
'                        TraceLog(3, "Casi_Badge: Changed status to " & BadgeReader("status") & " for badge number= " & bid)
'                    End If

'                Catch ex As Exception
'                    TraceLog(0, "Casi_Badge:Exception updating badge  number =" & bid & ":" & ex.Message)
'                End Try

'                informixCmd.Dispose()
'                Return
'            End If

'            Dim desc As String = BadgeReader("first_name").ToString.Trim + " " + BadgeReader("last_name").ToString
'            If desc.Length() > 60 Then desc = desc.Substring(0, 60)

'            Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
'               "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
'               "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
'               "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
'               "facility, modify_date, modify_time) " + _
'                "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

'            informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
'            informixCmd.Parameters.AddWithValue("description", desc)
'            informixCmd.Parameters.AddWithValue("bid", bidStr)
'            informixCmd.Parameters.AddWithValue("status", BadgeReader("status"))
'            informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("person_id", personid)
'            informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("issue_date", MakeInt(BadgeReader("issue_date")))
'            informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(BadgeReader("issue_time")))
'            informixCmd.Parameters.AddWithValue("issue_context", 1)
'            informixCmd.Parameters.AddWithValue("expired_date", MakeInt(BadgeReader("expired_date")))
'            informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(BadgeReader("expired_time")))
'            informixCmd.Parameters.AddWithValue("expired_context", 1)
'            informixCmd.Parameters.AddWithValue("return_date", MakeInt(BadgeReader("RETURN_DATE")))
'            informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(BadgeReader("RETURN_TIME")))
'            Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
'            informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(BadgeReader("RETURN_DATE")), DBNull.Value, Pacific_time))
'            informixCmd.Parameters.AddWithValue("usage_count", -1)
'            informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("bid_format_id", MakeInt(BadgeReader("BID_FORMAT_ID")))
'            informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
'            informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
'            informixCmd.Parameters.AddWithValue("unique_id", BadgeReader("BID").ToString)
'            informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
'            informixCmd.Parameters.AddWithValue("facility", -1)
'            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
'            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
'            Try
'                Dim NRows As Integer = informixCmd.ExecuteNonQuery()
'                If (NRows = 0) Then
'                    TraceLog(1, "Casi_Badge: Warning: failed to create new badge. badge = " & bid)
'                Else
'                    TraceLog(3, "Casi_Badge: Created new badge.  badge = " & bid)
'                End If

'            Catch ex As Exception
'                TraceLog(0, "Casi_Badge:Exception in inserting badge = " & bid & ":" & ex.ToString)
'            End Try

'            informixCmd.Dispose()

'        End While
'    End Sub
'    Sub TransferPerson(ByVal transactionNo As Integer)
'    End Sub
'    Sub TransferCompany(ByVal transactionNo As Integer)
'    End Sub
'    Sub TransferCategory(ByVal transactionNo As Integer)
'    End Sub
'    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
'        If (GlobalErrorlevel >= level) Then
'            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
'            My.Computer.FileSystem.WriteAllText(logDir + fileName, "L" & level & ": " & msg & vbCrLf, True)
'        End If
'    End Sub
'    Function MakeIntTime(ByVal t As Object) As Integer
'        If DBNull.Value.Equals(t) Then Return Nothing
'        Return Integer.Parse(t.ToString.Replace(":", ""))
'    End Function
'    Function MakeInt(ByVal t As Object) As Integer
'        If DBNull.Value.Equals(t) Then Return Nothing
'        Return Integer.Parse(t.ToString)
'    End Function
'End Class
''--------------------------------------------------------------------------
''    Sub aaa()
''        Casi_Department(laxidReader)
''        While True
''            Dim personid As Integer = Casi_Person(laxidReader)
''            If IsNothing(personid) Then
''                ' Log the error that person record is not created
''                Continue While
''            End If
''            Update_person_user(laxidReader, personid)
''            Update_person_category(laxidReader, personid)
''            ' Casi_Picture(laxidReader, personid)
''            Casi_Badge(laxidReader, personid)
''            SetTransmitDate(laxidReader)
''        End While
''        laxidConnection.Close()
''        InformixConnection.Close()
''        'Console.WriteLine("End Time=" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'Console.WriteLine("Time Spent Dept=" & deptTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Person=" & personTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Category=" & catTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent User=" & userTimer.Elapsed.ToString)
''        'Console.WriteLine("Time Spent Badge=" & badgeTimer.Elapsed.ToString)
''        'Console.ReadKey()

''        Dts.TaskResult = ScriptResults.Success

''    End Sub
''    Sub Casi_Department(ByRef laxidReader As SqlDataReader)   ' in the original returns new department ID in case of error returns -101
''        Dim dept As String = "9" & laxidReader("Dept")
''        '   needs PP401.proteus.informix.department laxid.dbo.division laxid.dbo.company
''        '   laxid.dbo.contact
''        'assuming databases are open

''        '1. select all lines where depatment = given dept
''        Dim refCmd As Odbc.OdbcCommand
''        Dim reader As Odbc.OdbcDataReader = Nothing
''        refCmd = New Odbc.OdbcCommand("select id from informix.department where user1 = ?", InformixConnection)
''        refCmd.Parameters.AddWithValue("user1", dept)
''        '2. if found exit. 
''        Try
''            reader = refCmd.ExecuteReader
''            If reader.Read Then
''                '    in the original code if department is not found it is not 
''                '    This may have to be readdressed.
''                reader.Close()
''                refCmd.Dispose()
''                TraceLog(3, "Casi_Department: department already exists ID = " & reader("id"))
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading department " & ex.ToString)
''        End Try

''        '3. else  find max id in table
''        Dim maxId As Integer = 0
''        refCmd = New Odbc.OdbcCommand("select max(id) as maxId from informix.department", InformixConnection)
''        Try
''            reader = refCmd.ExecuteReader
''            If reader.Read() Then
''                If Not reader.IsDBNull(0) Then maxId = reader.GetInt32(reader.GetOrdinal("maxId"))
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading maxId " & ex.ToString)
''        End Try
''        refCmd.Dispose()
''        reader.Close()
''        '4 increment Id
''        maxId += 1

''        '5. create a row with this id and other data from joining tables
''        Dim cmdText As String = "select " + _
''                       "company.print_name,division.name," + _
''                       "contact.fname,contact.lname,division.phone as phone, division.co_id, division.id," + _
''                       "company.ind_type " + _
''                      "from division left outer join company on division.co_id = company.id " + _
''                      "left outer join contact on division.co_id = contact.co_id and contact.div_id = division.id " + _
''                      "where status ='active'  and contact_type = 'BADGE1' " + _
''                      "and division.co_id = substring('" & dept & "',2,4) and division.id = substring('" & dept & "',6,2)"

''        Dim laxidConnection2 = New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        laxidConnection2.Open()

''        Dim laxidCmd As New SqlCommand(cmdText, laxidConnection2)
''        laxidCmd.Parameters.AddWithValue("Dept", dept.Trim())
''        laxidCmd.Parameters.AddWithValue("Dept1", dept.Trim())
''        Dim laxidReaderDept As SqlDataReader = Nothing
''        Try
''            laxidReaderDept = laxidCmd.ExecuteReader
''            If Not laxidReaderDept.Read() Then
''                laxidCmd.Dispose()
''                laxidReaderDept.Close()
''                Return ' some error to be handled????????????????????????????????????????????????????????
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department:Exception in Reading select " & ex.ToString)
''        End Try



''        '6. insert row in the table
''        Dim insCmdText As String = "INSERT INTO informix.department (" + _
''                           "id,description,division,location,manager,phone,user1,user2," + _
''                   "facility, modify_date, modify_time) VALUES (?,?,?,?,?,?,?,?,?,?,?)"

''        Dim insCmd As New Odbc.OdbcCommand(insCmdText, InformixConnection)
''        insCmd.Parameters.AddWithValue("id", maxId)
''        Dim desc As String = dept + " " + laxidReaderDept("print_name").trim()
''        If desc.Length > 50 Then desc = desc.Substring(0, 50)
''        insCmd.Parameters.AddWithValue("description", desc)
''        insCmd.Parameters.AddWithValue("division", DBNull.Value)
''        Dim loc As String = laxidReaderDept("name")
''        If loc.Length > 60 Then loc.Substring(0, 60).Trim()
''        insCmd.Parameters.AddWithValue("location", loc)
''        insCmd.Parameters.AddWithValue("manager", laxidReaderDept("fname").Trim() + " " + laxidReaderDept("lname").Trim())
''        insCmd.Parameters.AddWithValue("phone", laxidReaderDept("phone"))
''        insCmd.Parameters.AddWithValue("user1", "9" + laxidReaderDept("co_id").Trim + laxidReaderDept("id").Trim)
''        insCmd.Parameters.AddWithValue("user2", laxidReaderDept("ind_type").Trim)
''        insCmd.Parameters.AddWithValue("facility", -1)
''        insCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        insCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim Nrows As Integer = insCmd.ExecuteNonQuery()
''            If (Nrows = 0) Then
''                TraceLog(1, "Casi_Department: Warning: Failed to create department 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim)
''            Else
''                TraceLog(3, "Casi_Department: New department created 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim & "was inserted successfully")
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Department: Exception creating new department 9" & laxidReaderDept("co_id").Trim & laxidReaderDept("id").Trim & ":" & ex.ToString)
''        End Try

''    End Sub
''    Function Casi_Person(ByRef laxidReader As SqlDataReader) As Integer
''        Dim informixCmd = New Odbc.OdbcCommand("select id from informix.person where employee= ? and department= ?", InformixConnection)
''        '1. select all lines where employee= given employee and department= given department
''        informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''        informixCmd.Parameters.AddWithValue("department", laxidReader("dept"))
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Try
''            informixReader = informixCmd.ExecuteReader
''            '2. if found update
''            If informixReader.Read Then
''                Dim sqlText2 = "update informix.person set pin=? ,modify_date= ? ,modify_time= ? where person.status=0 and" + _
''                               " employee = ? and pin <> ?"
''                informixCmd = New Odbc.OdbcCommand(sqlText2, InformixConnection)
''                informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''                informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''                Try
''                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''                    If (NRows = 0) Then
''                        TraceLog(3, "Casi_Person: Person already exists Person ID =" & laxidReader("employee").ToString & "no change in pin number")
''                    Else
''                        TraceLog(3, "Casi_Person: Person already exists Person ID =" & laxidReader("employee").ToString & "pin number changed")
''                    End If
''                Catch ex As Exception
''                    TraceLog(0, "Casi_Person:Exception: failed to change pin for " & laxidReader("employee").ToString & ex.ToString)
''                End Try
''                ' update code                    not in the original ?????????????????????????????????
''                Dim idnumber As Integer = informixReader("id")
''                informixReader.Close()
''                informixCmd.Dispose()
''                Return idnumber
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in updating informix.person " & ex.ToString)
''        End Try

''        informixReader.Close()
''        informixCmd.Dispose()
''        '3. else  find max id in table
''        informixCmd = New Odbc.OdbcCommand("select max(id) as maxId from informix.person", InformixConnection)
''        Dim maxId As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader

''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in reading maxId " & ex.ToString)
''        End Try
''        '4 increment Id
''        maxId += 1
''        informixCmd.Dispose()
''        informixReader.Close()
''        '5. create a row with this id and other data from 
''        Dim cmdText As String = "INSERT INTO informix.person (" + _
''           "id,pin, status, type, person_kp_resp, person_trace, person_trace_alarm, " + _
''           "employee, department, first_name,last_name, initials, title, address1, address2, address3, address4,address5, " + _
''           "phone, phone2, 	reissue_cnt, apb, reader, access_date,access_time, access_tz, " + _
''           "active_date, active_time, active_context, deactive_date, deactive_time, deactive_context, force_download, " + _
''            "facility, modify_date, modify_time) " + _
''            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

''        informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
''        informixCmd.Parameters.AddWithValue("id", maxId)
''        informixCmd.Parameters.AddWithValue("pin", laxidReader("pin"))
''        informixCmd.Parameters.AddWithValue("status", 0)
''        informixCmd.Parameters.AddWithValue("type", 1)
''        informixCmd.Parameters.AddWithValue("person_kp_resp", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_trace", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_trace_alarm", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("employee", laxidReader("employee"))
''        informixCmd.Parameters.AddWithValue("department", laxidReader("dept"))
''        informixCmd.Parameters.AddWithValue("first_name", laxidReader("first_name"))
''        informixCmd.Parameters.AddWithValue("last_name", laxidReader("last_name"))
''        informixCmd.Parameters.AddWithValue("initials", laxidReader("initials"))
''        informixCmd.Parameters.AddWithValue("title", laxidReader("user6"))
''        informixCmd.Parameters.AddWithValue("address1", laxidReader("address1"))
''        informixCmd.Parameters.AddWithValue("address2", laxidReader("address2"))
''        informixCmd.Parameters.AddWithValue("address3", laxidReader("address3"))
''        informixCmd.Parameters.AddWithValue("address4", laxidReader("address4"))
''        informixCmd.Parameters.AddWithValue("address5", laxidReader("address5"))
''        informixCmd.Parameters.AddWithValue("phone", laxidReader("phone"))
''        informixCmd.Parameters.AddWithValue("phone2", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("reissue_cnt", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("apb", 0)
''        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("active_date", 19710101)
''        informixCmd.Parameters.AddWithValue("active_time", 235959)
''        informixCmd.Parameters.AddWithValue("active_context", 1)
''        informixCmd.Parameters.AddWithValue("deactive_date", 20201231)
''        informixCmd.Parameters.AddWithValue("deactive_time", 235959)
''        informixCmd.Parameters.AddWithValue("deactive_context", 1)
''        informixCmd.Parameters.AddWithValue("force_download", 1)
''        informixCmd.Parameters.AddWithValue("facility", -1)
''        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Person:warning: failed to create New Person ID = " & laxidReader("employee"))
''            Else
''                TraceLog(3, "Casi_Person: New Person created successfully.  personID = " & laxidReader("employee"))
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Person:Exception in inserting Person ID for ID= " & laxidReader("employee").ToString & ":" & ex.ToString)
''            Return Nothing
''        End Try
''        informixCmd.Dispose()
''        Return maxId
''    End Function
''    Public Sub Casi_Picture(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        '
''        ' Check if image already exists for this person
''        Dim getPicCmd As New Odbc.OdbcCommand("SELECT * from images where person_id=?", InformixConnection)
''        getPicCmd.Parameters.AddWithValue("person_id", PersonId)
''        Dim getPicReader As Odbc.OdbcDataReader = Nothing
''        Try
''            getPicReader = getPicCmd.ExecuteReader()
''            If getPicReader.Read Then
''                ' Picture already exists for the person. (TBD: should it be overwritten?) for now do not update
''                getPicReader.Close()
''                getPicCmd.Dispose()
''                TraceLog(3, "Casi_Picture: Picture already exists")
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in Reading image from PP. " & ex.ToString)
''        End Try

''        Dim cmdFrom As New SqlCommand("SELECT Picture FROM Person WHERE EMP_ID=@EmployeeID", picConnection)
''        cmdFrom.Parameters.AddWithValue("EmployeeID", laxidReader("EMPLOYEE"))
''        Dim pictureReader As SqlDataReader = Nothing
''        Try
''            pictureReader = cmdFrom.ExecuteReader()
''            If Not pictureReader.Read Then
''                ' person record does not exitst in b2k. should not happen
''                pictureReader.Close()
''                cmdFrom.Dispose()
''                TraceLog(0, "Casi_Picture:Person Record not found in B2K for EMP_ID=" & laxidReader("EMPLOYEE"))
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in Reading person from B2K " & ex.ToString)
''        End Try


''        If pictureReader.IsDBNull(0) Then
''            ' picture is null in b2k
''            Return
''        End If

''        Dim insertCmd As New Odbc.OdbcCommand("INSERT INTO images (person_id, type, size, width, height, version, compression, image_data, creation_date, creation_time, modify_date, modify_time) " & _
''                                                " VALUES (?, 0, ?, 140, 160, '2.4.2',1, ?, ?,?,?,?)", InformixConnection)
''        Dim image() As Byte = pictureReader("Picture")
''        pictureReader.Close()
''        cmdFrom.Dispose()

''        image = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, System.Text.Encoding.Default, image)
''        insertCmd.Parameters.Clear()
''        insertCmd.Parameters.AddWithValue("person_id", PersonId)
''        insertCmd.Parameters.AddWithValue("size", image.Length)
''        insertCmd.Parameters.AddWithValue("image_data", image)
''        insertCmd.Parameters.AddWithValue("creation_date", Now.ToString("yyyyMMdd"))
''        insertCmd.Parameters.AddWithValue("creation_time", Now.ToString("HHmmss"))
''        insertCmd.Parameters.AddWithValue("modify_date", Now.ToString("yyyyMMdd"))
''        insertCmd.Parameters.AddWithValue("modify_time", Now.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = insertCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Picture: No pictures inserted informix.person")
''            Else
''                TraceLog(3, "Casi_Picture: Picture transferred to pp4 successfully. Person ID = " & laxidReader("EMPLOYEE"))
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Casi_Picture:Exception in inserting Image for PersonID=" & PersonId & " : " & ex.Message)
''        End Try
''        insertCmd.Dispose()
''        '
''        Dts.TaskResult = ScriptResults.Success
''    End Sub

''    Sub Casi_Badge(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        ' TBD check for personID=nothing. log error that badge cannot be created

''        Dim bid As String = laxidReader("bid")
''        Dim bidStr As String
''        bid = bid.Trim
''        If bid.Length < 7 Then
''            bidStr = "00520" + bid
''        Else
''            bidStr = "00101" + bid
''        End If

''        Dim informixCmd = New Odbc.OdbcCommand("select id from informix.badge where bid= ? ", InformixConnection)
''        '1. select all lines where employee= given employee and department= given department
''        informixCmd.Parameters.AddWithValue("bid", bidStr)
''        Try
''            Dim informixReader As Odbc.OdbcDataReader = informixCmd.ExecuteReader
''            If informixReader.Read Then
''                Dim sqlText As String = "update informix.badge set return_date= ?, return_time= ?,return_tz= ?" + _
''                                        ",status=?,modify_date= ?,modify_time= ? where unique_id = ?"
''                informixCmd = New Odbc.OdbcCommand(sqlText, InformixConnection)
''                informixCmd.Parameters.AddWithValue("return_date", MakeInt(laxidReader("RETURN_DATE")))
''                informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidReader("RETURN_TIME")))
''                informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidReader("RETURN_DATE")), DBNull.Value, 342))
''                informixCmd.Parameters.AddWithValue("status", laxidReader("status"))
''                informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                informixCmd.Parameters.AddWithValue("unique_id", bidStr)
''                Try
''                    Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''                    If (NRows = 0) Then
''                        TraceLog(1, "Casi_Badge: warning: Failed to change status to " & laxidReader("status") & " for badge number= " & bid)
''                    Else
''                        TraceLog(3, "Casi_Badge: Changed status to " & laxidReader("status") & " for badge number= " & bid)
''                    End If

''                Catch ex As Exception
''                    TraceLog(0, "Casi_Badge:Exception updating badge  number =" & bid & ":" & ex.Message)
''                End Try

''                informixCmd.Dispose()
''                Return
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Casi_Badge:Exception in reading informix.badge " & ex.Message)
''        End Try

''        Dim desc As String = laxidReader("first_name").ToString.Trim + " " + laxidReader("last_name").ToString
''        If desc.Length() > 60 Then desc = desc.Substring(0, 60)

''        Dim cmdText As String = "INSERT INTO informix.badge (description, bid, status, badge_tour, badge_temp, " + _
''           "person_id, reader, access_date, access_time, access_tz, issue_date, issue_time,issue_context, " + _
''           "expired_date, expired_time,expired_context, return_date, return_time,return_tz, usage_count," + _
''           "usage_exhausted, tour_badge, bid_format_id, reissue_cnt,reprint_cnt, unique_id, badge_design, " + _
''           "facility, modify_date, modify_time) " + _
''            "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

''        informixCmd = New Odbc.OdbcCommand(cmdText, InformixConnection)
''        informixCmd.Parameters.AddWithValue("description", desc)
''        informixCmd.Parameters.AddWithValue("bid", bidStr)
''        informixCmd.Parameters.AddWithValue("status", laxidReader("status"))
''        informixCmd.Parameters.AddWithValue("badge_tour", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("badge_temp", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        informixCmd.Parameters.AddWithValue("reader", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_date", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_time", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("access_tz", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("issue_date", MakeInt(laxidReader("issue_date")))
''        informixCmd.Parameters.AddWithValue("issue_time", MakeIntTime(laxidReader("issue_time")))
''        informixCmd.Parameters.AddWithValue("issue_context", 1)
''        informixCmd.Parameters.AddWithValue("expired_date", MakeInt(laxidReader("expired_date")))
''        informixCmd.Parameters.AddWithValue("expired_time", 235959) 'MakeIntTime(laxidReader("expired_time")))
''        informixCmd.Parameters.AddWithValue("expired_context", 1)
''        informixCmd.Parameters.AddWithValue("return_date", MakeInt(laxidReader("RETURN_DATE")))
''        informixCmd.Parameters.AddWithValue("return_time", MakeIntTime(laxidReader("RETURN_TIME")))
''        Dim Pacific_time As Integer = 342  ' the code for pacific time in ????????
''        informixCmd.Parameters.AddWithValue("return_tz", IIf(DBNull.Value.Equals(laxidReader("RETURN_DATE")), DBNull.Value, Pacific_time))
''        informixCmd.Parameters.AddWithValue("usage_count", -1)
''        informixCmd.Parameters.AddWithValue("usage_exhausted", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("tour_badge", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("bid_format_id", MakeInt(laxidReader("BID_FORMAT_ID")))
''        informixCmd.Parameters.AddWithValue("reissue_cnt", "00")
''        informixCmd.Parameters.AddWithValue("reprint_cnt", "00")
''        informixCmd.Parameters.AddWithValue("unique_id", laxidReader("BID").ToString)
''        informixCmd.Parameters.AddWithValue("badge_design", DBNull.Value)
''        informixCmd.Parameters.AddWithValue("facility", -1)
''        informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''        informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''        Try
''            Dim NRows As Integer = informixCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "Casi_Badge: Warning: failed to create new badge. badge = " & bid)
''            Else
''                TraceLog(3, "Casi_Badge: Created new badge.  badge = " & bid)
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Casi_Badge:Exception in inserting badge = " & bid & ":" & ex.ToString)
''        End Try

''        informixCmd.Dispose()
''    End Sub
''    Sub Update_person_user(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        'Dim stopWatch1 As New System.Diagnostics.Stopwatch
''        ''Console.WriteLine("======person_user before count========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        Dim informixCmd = New Odbc.OdbcCommand("select count(*) as userCount from informix.person_user where person_id = ?", InformixConnection)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Dim recCount As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then recCount = informixReader.GetDecimal(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_user:Exception failed to count the number of records for personID = " & PersonId & ":" & ex.ToString)
''        End Try

''        'Console.WriteLine("======person_user after count========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  count {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        informixReader.Close()
''        informixCmd.Dispose()
''        If recCount > 0 Then
''            TraceLog(1, "Update_person_user:Warning: person ID = " & PersonId.ToString & " already exists")
''            '  no upgrades are done ??????????????????????????????????????????????????
''            Return
''        End If
''        ' no user data found so fill it
''        ' 1. find largest id  in person-user
''        'Console.WriteLine("======person_user before maxid========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        informixCmd = New Odbc.OdbcCommand("select  max(id) from informix.person_user", InformixConnection)
''        Dim maxRowId As Integer = 0
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxRowId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_user:Exception in reading maxId from informix.person_user " & ex.ToString)
''        End Try

''        'Console.WriteLine("======person_user after maxid========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  maxid {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        '' 3. loop i=1 to 40   read user-i 
''        ''     increment largest id
''        ''     If useri null continue 
''        ''     insert in slot number i
''        ''     fill the user-i 
''        ''    end loop  
''        ''Console.WriteLine("======person_user before insert loop========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Start()
''        Dim NRows As Integer
''        For i = 1 To 40
''            If DBNull.Value.Equals(laxidReader("user" & i)) Then
''                Continue For
''            End If
''            maxRowId += 1
''            informixCmd = New Odbc.OdbcCommand("insert into informix.person_user(id,description,person_id," + _
''                                               "slot_number,facility,modify_date,modify_time) " + _
''                                               "values(?,?,?,?,?,?,?)", InformixConnection)
''            informixCmd.Parameters.AddWithValue("id", maxRowId)
''            informixCmd.Parameters.AddWithValue("description", laxidReader("user" & i.ToString))
''            informixCmd.Parameters.AddWithValue("person_id", PersonId)
''            informixCmd.Parameters.AddWithValue("slot_number", i)
''            informixCmd.Parameters.AddWithValue("facility", -1)
''            informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''            informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''            Try
''                informixCmd.ExecuteNonQuery()
''                NRows = informixCmd.ExecuteNonQuery()
''                If (NRows = 0) Then
''                    TraceLog(1, "Update_person_user: warning: failed to insert slot " & i & " for personID = " & PersonId)
''                Else
''                    TraceLog(4, "Update_person_user: successfully inserted slot " & i & " for personID = " & PersonId)
''                End If

''            Catch ex As Exception
''                TraceLog(0, "Update_person_user:Exception  inserting  slot " & i & " for personID = " & PersonId & ":" & ex.ToString)
''            End Try

''        Next
''        '  Tr
''        ''Console.WriteLine("======person_user after insert loop========" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
''        'stopWatch1.Stop()
''        'Console.WriteLine("  inserts {0}  milliseconds ", stopWatch1.ElapsedMilliseconds.ToString)
''        ' 4.
''    End Sub
''    Sub Update_person_category(ByRef laxidReader As SqlDataReader, ByVal PersonId As Integer)
''        ' no user data found so fill it
''        ' 1. find largest id  in person-user
''        Dim informixCmd = New Odbc.OdbcCommand("select  max(id) from informix.person_category", InformixConnection)
''        Dim maxRowId As Integer = 0
''        Dim informixReader As Odbc.OdbcDataReader = Nothing
''        Try
''            informixReader = informixCmd.ExecuteReader
''            If informixReader.Read() Then
''                If Not informixReader.IsDBNull(0) Then maxRowId = informixReader.GetInt32(0)
''            End If
''        Catch ex As Exception
''            TraceLog(0, "Update_person_category:Exception in reading maxId from informix.person_category" & ex.Message)
''        End Try

''        informixReader.Close()
''        informixCmd.Dispose()
''        '2. find all slots belonging to a person
''        informixCmd = New Odbc.OdbcCommand("select slot_number from informix.person_category where person_id = ? order by slot_number", InformixConnection)
''        informixCmd.Parameters.AddWithValue("person_id", PersonId)
''        Try
''            informixReader = informixCmd.ExecuteReader
''        Catch ex As Exception
''            TraceLog(0, "Update_person_category:Exception in reading informix.person_category " & ex.Message)
''        End Try


''        Dim found As Boolean
''        Dim foundSlot As Integer
''        Dim slotNum As Integer
''        If informixReader.Read() Then
''            slotNum = informixReader("slot_number")
''        Else
''            slotNum = 9999
''        End If
''        Dim NRows As Integer
''        For i = 1 To 50
''            found = False
''            If (slotNum = i) Then
''                found = True
''                If informixReader.Read() Then
''                    slotNum = informixReader("slot_number")
''                Else
''                    slotNum = 9999
''                End If
''            End If

''            If (found) Then
''                '  not found in input therefore delete slot
''                If DBNull.Value.Equals(laxidReader("Category" & i.ToString)) Then
''                    informixCmd = New Odbc.OdbcCommand("delete from informix.person_category where person_id= ? and slot_number = ?", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("slot_number", foundSlot)
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category:warning: Failed to delete slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Deleted slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        End If
''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception in deleting slot_number " & foundSlot.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                Else
''                    ' found in informix therefore update slot
''                    informixCmd = New Odbc.OdbcCommand("update informix.person_category set category_id=?,modify_date=?,modify_time=? " + _
''                                 " where person_id= ? and slot_number=?", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("category_id", laxidReader("category" & i.ToString))
''                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("slot_number", i)
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category:warning: Failed to update slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Updated slot_number " & foundSlot.ToString & " for person_id= " & PersonId)
''                        End If
''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception in updating slot_number " & foundSlot.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                End If
''            Else ' not found in informix therefore insert slot
''                If Not DBNull.Value.Equals(laxidReader("Category" & i.ToString)) Then
''                    maxRowId += 1
''                    informixCmd = New Odbc.OdbcCommand("insert into informix.person_category(id,person_id," + _
''                                                   "category_id,slot_number,facility,modify_date,modify_time) " + _
''                                                   "values(?,?,?,?,?,?,?)", InformixConnection)
''                    informixCmd.Parameters.AddWithValue("id", maxRowId)
''                    informixCmd.Parameters.AddWithValue("person_id", PersonId)
''                    informixCmd.Parameters.AddWithValue("category_id", laxidReader("category" & i.ToString))
''                    informixCmd.Parameters.AddWithValue("slot_number", i)
''                    informixCmd.Parameters.AddWithValue("facility", -1)
''                    informixCmd.Parameters.AddWithValue("modify_date", DateTime.UtcNow.ToString("yyyyMMdd"))
''                    informixCmd.Parameters.AddWithValue("modify_time", DateTime.UtcNow.ToString("HHmmss"))
''                    Try
''                        NRows = informixCmd.ExecuteNonQuery()
''                        If (NRows = 0) Then
''                            TraceLog(1, "Update_person_category: warning: Failed to insert slot_number " & i.ToString & " for person_id= " & PersonId)
''                        Else
''                            TraceLog(4, "Update_person_category: Inserted slot_number " & i.ToString & " for person_id= " & PersonId)
''                        End If

''                    Catch ex As Exception
''                        TraceLog(0, "Update_person_category:Exception inserting slot_number " & i.ToString & " for person_id= " & PersonId & ":" & ex.ToString)
''                    End Try

''                End If
''            End If
''        Next

''        ' 4.
''    End Sub
''    Sub SetTransmitDate(ByRef laxidReader As SqlDataReader)

''        'Dim laxidConnectionUpdate As SqlConnection = New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        'laxidConnectionUpdate.Open()
''        laxidManager = Dts.Connections("LaxBadgeSql.LAXID")
''        Dim laxidConnectionUpdate As SqlConnection = laxidManager.AcquireConnection(Nothing) 'New SqlConnection("server=sbdgsql02;Integrated Security=True;database=laxid")
''        Dim laxidCmd As SqlCommand
''        laxidCmd = New SqlCommand("Update B2KCASI4 set transmit_dt = @transmit_dt where id = @id", laxidConnectionUpdate)
''        laxidCmd.Parameters.AddWithValue("transmit_dt", DateTime.Now.ToString)
''        laxidCmd.Parameters.AddWithValue("id", laxidReader("ID"))
''        Try
''            Dim NRows As Integer = laxidCmd.ExecuteNonQuery()
''            If (NRows = 0) Then
''                TraceLog(1, "warning: failed to set transmission date for id =" & laxidReader("ID").ToString)
''            Else
''                TraceLog(3, "successfully setted  transmission date for id =" & laxidReader("ID").ToString)
''            End If

''        Catch ex As Exception
''            TraceLog(0, "Exception in setting transmit_dt field in B2KCASI4 for id = " & laxidReader("ID").ToString & ":" & ex.ToString)
''        End Try
''        laxidCmd.Dispose()
''        laxidConnectionUpdate.Close()
''    End Sub
''    Private Sub TraceLog(ByVal level As Integer, ByVal msg As String)
''        If (GlobalErrorlevel >= level) Then
''            Dim fileName As String = "ACAMSTransfer" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"
''            My.Computer.FileSystem.WriteAllText(logDir + fileName, "L" & level & ": " & msg & vbCrLf, True)
''        End If
''    End Sub
''End Class]]></ProjectItem>
          <ProjectItem
            Name="My Project\Resources.resx"
            Encoding="UTF8"><![CDATA[<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>]]></ProjectItem>
          <ProjectItem
            Name="My Project\Settings.settings"
            Encoding="UTF8"><![CDATA[<?xml version='1.0' encoding='iso-8859-1'?>
<SettingsFile xmlns="uri:settings" CurrentProfile="(Default)" GeneratedClassNamespace="$safeprojectname" GeneratedClassName="MySettings">
  <Profiles>
    <Profile Name="(Default)" />
  </Profiles>
  <Settings />
</SettingsFile>]]></ProjectItem>
          <ProjectItem
            Name="\my project\assemblyinfo.vb"
            Encoding="UTF8"><![CDATA[Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("ST_fee19305817346c48778eab4d49c9257.vbproj")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("ST_fee19305817346c48778eab4d49c9257.vbproj")> 
<Assembly: AssemblyCopyright("Copyright @ Microsoft 2010")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 

<Assembly: ComVisible(False)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("c20cc24e-8008-4f27-b8aa-920f991bada3")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> ]]></ProjectItem>
          <ProjectItem
            Name="\my project\resources.designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On


Namespace My.Resources
    
    '''<summary>
    '''   A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    'This class was auto-generated by the Strongly Typed Resource Builder
    'class via a tool like ResGen or Visual Studio.NET.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    Class MyResources
        
        Private Shared _resMgr As System.Resources.ResourceManager
        
        Private Shared _resCulture As System.Globalization.CultureInfo
        
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''   Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared ReadOnly Property ResourceManager() As System.Resources.ResourceManager
            Get
                If (_resMgr Is Nothing) Then
                    Dim temp As System.Resources.ResourceManager = New System.Resources.ResourceManager("My.Resources.MyResources", GetType(MyResources).Assembly)
                    _resMgr = temp
                End If
                Return _resMgr
            End Get
        End Property
        
        '''<summary>
        '''   Overrides the current thread's CurrentUICulture property for all
        '''   resource lookups using this strongly typed resource class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared Property Culture() As System.Globalization.CultureInfo
            Get
                Return _resCulture
            End Get
            Set
                _resCulture = value
            End Set
        End Property
    End Class
End Namespace]]></ProjectItem>
          <BinaryItem
            Name="ST_fee19305817346c48778eab4d49c9257.vbproj.dll">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
AAAKb0cAAAomCG9GAAAKchggAHB+bQAACm9HAAAKJghvRgAACnIyIABwfm0AAApvRwAACiYIb0YA
AApyKBIAcANygRcAcG84AAAKKFcAAApvRwAACiYIb0YAAApyWCAAcAcW/gF+bQAACgeMOgAAAShv
AAAKKFcAAApvRwAACiYIb0YAAApyfBkAcANykhkAcG84AAAKKFcAAApvRwAACiYIb0YAAApynhkA
cANyshkAcG84AAAKKFcAAApvRwAACiYIb0YAAApyvhkAcANy0BkAcG84AAAKKFcAAApvRwAACiYI
b0YAAApy6hkAcANy/BkAcG84AAAKb0MAAApvRwAACiYIb0YAAApyChoAcANyHBoAcG84AAAKb0MA
AApvRwAACiYIb0YAAApyKBoAcANyOhoAcG84AAAKb0MAAApvRwAACiYIb0YAAApyRBoAcANyVhoA
cG84AAAKb0MAAApvRwAACiYIb0YAAApyYhoAcANydBoAcG84AAAKb0MAAAoDcnwaAHBvOAAACm9D
AAAKKDMAAApvRwAACiYIb0YAAApycgkAcANyjBoAcG84AAAKKFcAAApvRwAACiYIb0YAAApybiAA
cH5tAAAKb0cAAAomCG9GAAAKcnwgAHB+bQAACm9HAAAKJghvRgAACnKUIABwFow6AAABb0cAAAom
CG9GAAAKcpwgAHB+bQAACm9HAAAKJghvRgAACnKqIABwfm0AAApvRwAACiYIb0YAAApywiAAcH5t
AAAKb0cAAAomCG9GAAAKctogAHB+bQAACm9HAAAKJghvRgAACnLuIABwIJXALAGMOgAAAW9HAAAK
JghvRgAACnIGIQBwILeZAwCMOgAAAW9HAAAKJghvRgAACnIeIQBwF4w6AAABb0cAAAomCG9GAAAK
cjwhAHAgDz80AYw6AAABb0cAAAomCG9GAAAKclghAHAgt5kDAIw6AAABb0cAAAomCG9GAAAKcnQh
AHAXjDoAAAFvRwAACiYIb0YAAApyliEAcBaMOgAAAW9HAAAKJghvRgAACnK4CQBwFYw6AAABb0cA
AAomCG9GAAAKcsoJAHAoUgAAChMFEgVy4gkAcChTAAAKb0cAAAomCG9GAAAKcvQJAHAoUgAAChMF
EgVyDAoAcChTAAAKb0cAAAomFhMECG9UAAAKEwQRBC0zAhdytCEAcARyVBMAcCgzAAAKA3KBFwBw
bzgAAAooTgAACihLAAAKKEwAAAooHwAABisxAhly+CEAcARyVBMAcCgzAAAKA3KBFwBwbzgAAAoo
TgAACihLAAAKKEwAAAooHwAABt5aJSgxAAAKEwYCFhyNOQAAASUWciAiAHCiJRcEoiUYclQTAHCi
JRkDcoEXAHBvOAAACm9DAAAKoiUacggLAHCiJRsRBm8yAAAKoihwAAAKKB8AAAYoNAAACt4ACG86
AAAKEQQWMQ0CAnsMAAAEAygZAAAGBioAAAEQAAAAAEAEcrIEWisAAAETMAcAkAAAAAgAABFyXiIA
cAJ7EQAABHM1AAAKJW89AAAKct0GAHADjDoAAAFvPgAACiZvNgAACgorUgIGKBYAAAYLBxYxRgIZ
G405AAABJRZy2CIAcKIlFwZygRcAcG84AAAKb0MAAAqiJRhy8CIAcKIlGQcoPAAACqIlGnL2IgBw
oihwAAAKKB8AAAYGbzcAAAotpgZvOwAACiobMAwAcQMAAAkAABEEcoEXAHBvOAAACm9DAAAKC3IY
IwBwAnsTAAAEc0UAAAoKBm9GAAAKcigSAHAHb0cAAAomFgwGb2IAAAooTAAAChICKGMAAAomCC0T
AhdyZiMAcAcoMwAACigfAAAGKgZvOgAACgNvcQAACg049AIAAAlvXwAACiUtDSYSBf4VMwAAAREF
KwWlMwAAARMEEgQocgAACig5AAAKEwYEFHKuIwBwF40YAAABJRYSBChzAAAKoiUTCBQUF41LAAAB
JRYXnCUTCShNAAAKEQkWkSwVEgQRCBaaKFcAAAooVwAACih0AAAKb0MAAAoTB3K4IwBwAnsUAAAE
c0UAAAoKBm9GAAAKcg4JAHAEFHKuIwBwF40YAAABJRYSBChzAAAKoiUTCBQUF41LAAABJRYXnCUT
CShNAAAKEQkWkSwVEgQRCBaaKFcAAAooVwAACih0AAAKKFcAAApvRwAACiYGb0YAAApyygkAcChS
AAAKEwoSCnLiCQBwKFMAAApvRwAACiYGb0YAAApy9AkAcChSAAAKEwoSCnIMCgBwKFMAAApvRwAA
CiYGb0YAAApyOhIAcAiMOgAAAW9HAAAKJgZvRgAACnJmEgBwEgQocgAACihXAAAKb0cAAAomBm9U
AAAKFj08AQAABm86AAAKcockAHACexQAAARzRQAACgoGb0YAAApyzwcAcAgfZNiMOgAAARIEKHIA
AAooTgAACm9HAAAKJgZvRgAACnIOCQBwBBRyriMAcBeNGAAAASUWEgQocwAACqIlEwgUFBeNSwAA
ASUWF5wlEwkoTQAAChEJFpEsFRIEEQgWmihXAAAKKFcAAAoodAAACihXAAAKb0cAAAomBm9GAAAK
cjoSAHAIjDoAAAFvRwAACiYGb0YAAApyZhIAcBIEKHIAAAooVwAACm9HAAAKJgZvRgAACnK4CQBw
FYw6AAABb0cAAAomBm9GAAAKcsoJAHAoUgAAChMKEgpy4gkAcChTAAAKb0cAAAomBm9GAAAKcvQJ
AHAoUgAAChMKEgpyDAoAcChTAAAKb0cAAAomBm9UAAAKJgZvOgAACt5DJSgxAAAKEwsCF3KCJQBw
Go0YAAABJRYHoiUXEQaMOgAAAaIlGBEHoiUZEQtvZQAACqIoZAAACigfAAAGKDQAAAreAAlvZwAA
CjoB/f//KgAAAEEcAAAAAAAA7AAAADYCAAAiAwAAQwAAACsAAAEbMAYAewIAAAoAABFy8wYAcARy
9wYAcG84AAAKb0MAAAoEchEmAHBvOAAACm9DAAAKKEQAAAoKAgRygRcAcG84AAAKb0MAAAoGKB4A
AAYLBHIfJgBwbzgAAApvQwAACm9RAAAKDHIrJgBwDQkCexQAAARzRQAAChMEEQRvRgAACnIOCQBw
CG9HAAAKJhEEb0YAAApyOhIAcAeMOgAAAW9HAAAKJhEEb0YAAApyXicAcAIEcnYnAHBvOAAACihX
AAAKKCEAAAYoVwAACm9HAAAKJhEEb0YAAApyiicAcAIEcnYnAHBvOAAACihXAAAKKCAAAAYoVwAA
Cm9HAAAKJhEEb0YAAApyoicAcH5tAAAKBHJ2JwBwbzgAAAooVwAACm91AAAKfm0AAAogVgEAAIw6
AAABKG8AAAooVwAACm9HAAAKJhEEb0YAAApy4h8AcAJ7CwAABARy4h8AcG84AAAKb0MAAApvdgAA
CihXAAAKb0cAAAomEQRvRgAACnLKCQBwKFIAAAoTBhIGcuIJAHAoUwAACm9HAAAKJhEEb0YAAApy
9AkAcChSAAAKEwYSBnIMCgBwKFMAAApvRwAACiYRBG9GAAAKcrYnAHADb0cAAAomFhMFEQRvVAAA
ChMFEQUtLgIXcr4nAHADcv4nAHAoRAAACgRy4h8AcG84AAAKKEsAAAooTAAACigfAAAGKywCGXIS
KABwA3L+JwBwKEQAAAoEcuIfAHBvOAAACihLAAAKKEwAAAooHwAABt4tJSgxAAAKEwcCFnJAKABw
A3IICwBwEQdvZQAACihVAAAKKB8AAAYoNAAACt4AEQRvOgAAChEFFjENAgJ7DQAABAQoGQAABhEF
KgABEAAAAADJAWkyAi0rAAABGzAGAKIEAAALAAARcvMGAHAEcvcGAHBvOAAACm9DAAAKBHIRJgBw
bzgAAApvQwAACihEAAAKCgIEcoEXAHBvOAAACm9DAAAKBigeAAAGCwRyHyYAcG84AAAKb0MAAApv
UQAACgxygigAcA0JAnsUAAAEc0UAAAoTBBEEb0YAAApyDgkAcAhvRwAACiYRBG9GAAAKcrYnAHAD
b0cAAAomEQRvRgAACnLiHwBwAnsLAAAEBHLiHwBwbzgAAApvQwAACm92AAAKKFcAAApvRwAACiYR
BG9GAAAKciEsAHB+bQAACm9HAAAKJhEEb0YAAApyNywAcH5tAAAKb0cAAAomEQRvRgAACnI6EgBw
B4w6AAABb0cAAAomEQRvRgAACnKcIABwfm0AAApvRwAACiYRBG9GAAAKcqogAHB+bQAACm9HAAAK
JhEEb0YAAApywiAAcH5tAAAKb0cAAAomEQRvRgAACnLaIABwfm0AAApvRwAACiYRBG9GAAAKck0s
AHACBHJjLABwbzgAAAooVwAACighAAAGKFcAAApvRwAACiYRBG9GAAAKcnUsAHACBHJjLABwbzgA
AAooVwAACiggAAAGKFcAAApvRwAACiYRBG9GAAAKcossAHAXjDoAAAFvRwAACiYRBG9GAAAKcqcs
AHACBHLBLABwbzgAAAooVwAACighAAAGKFcAAApvRwAACiYRBG9GAAAKcs8sAHAgt5kDAIw6AAAB
b0cAAAomEQRvRgAACnLpLABwF4w6AAABb0cAAAomEQRvRgAACnJeJwBwAgRydicAcG84AAAKKFcA
AAooIQAABihXAAAKb0cAAAomEQRvRgAACnKKJwBwAgRydicAcG84AAAKKFcAAAooIAAABihXAAAK
b0cAAAomIFYBAAATBREEb0YAAApyoicAcH5tAAAKBHJ2JwBwbzgAAAooVwAACm91AAAKfm0AAAoR
BYw6AAABKG8AAAooVwAACm9HAAAKJhEEb0YAAApyCS0AcBWMOgAAAW9HAAAKJhEEb0YAAApyIS0A
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
IiEAAAAAFghhDpwDAgAsIQAAAAAGGIALBgACAKghAAAAAAYACQgGAAIATCYAAAAABgAOCAEAAgAY
LAAAAAAGAJwPAQADAIAuAAAAAAYAUQ2hAwQAmDIAAAAABgCGDwEABwCkMwAAAAAGAK0LqgMIABA2
AAAAAAYA3wGxAwkAVDsAAAAABgBPCQEACwDwOwAAAAAGAL8AuQMMAIw/AAAAAAYAuwHDAw4AJEIA
AAAABgDNAcMDEADkRgAAAAAGAEsCAQASACRIAAAAAAYAVwDLAxMAkEgAAAAABgBIAMsDFQA8SQAA
AAABAHsG0QMXANhJAAAAAAYAqwKhABkALEoAAAAABgBmA6EAGgB+SgAAAADGAnUNswEbAIxKAAAA
AMYCKgLqABwAlEoAAAAAgwDXAtcDHACgSgAAAADGAmgGrQAcAKhKAAAAABEAmADcAxwAyEoAAAAA
AQCEAOQDHQC6IAAAAAAGGIALBgAeANRKAAAAAAMIAQJKAB4ACEsAAAAABhiACwYAHgAAAAEAHQYA
AAEAOgAAAAEAOgAAAAEACQ4AAAIAmQEAAAMA7Q4AAAEAOgAAAAEA4AkAAAEA4AkAAAIA7Q4AAAEA
OgAAAAEAjQkAAAIA4AkAAAEAoAEAAAIAzwkAAAEAoAEAAAIAzwkAAAEAOgAAAAEANgIAAAIAHwgA
AAEANgIAAAIA7Q4AAAEAfgcAAAIAhAYAAAEA6w4AAAEA6w4AAAEAaAkAAAEAIQIAAAEAIQIJAIAL
AQARAIALBgAZAIALCgApAIALEAAxAIALEAA5AIALEABBAIALEABJAIALEABRAIALEABZAIALFQBh
AIALFQBpAIALEABxAIALEAB5AIALEACBAIALGgCRAIALIACpAIALBgCxAIALBgC5AIALBgDRAIAL
JgDpAIALEAABAYALBgAJAYALBgAZAYALBgCZAIALBgChAIALBgAMAIALBgAUAIALBgAcAIALBgAk
AIALBgAMAAECSgAUAAECSgAcAAECSgAkAAECSgDBAIALBgDZAJACTwDZAFEPVwDxAIALXQC5AYwB
ZQARAYALBgAhAYALBgApAYALBgDJAVsOgQApARcBiAAhAQEOjgDZAaQNlADhAbwHmgAxAQ0JoQDp
ATYLpgBZAWgGrQDJAR8OsQDpASQLtwBRAYALuwBRAfcJwwBhARIByQBhAbwHzQDxAagK0gD5AVUD
BgBhAU8DBgDxAWgG1wBRAcMN3AABAvkF4gBRAV4P6gA5AU8DBgBBAU8DBgDZAW0OAQDBAGgGrQDJ
AR8OFgFpAYALHQFpAcMNJQERAvkFKwFpAfcJMwFxARIByQBxAbwHzQAhAjcOOQHxAWgGPwEpAlMO
RAEhAi0OOQHJAY0G6gDJAXEGVQHJAdQHrQB5AfwOWwF5AWgGYQFpAV4P6gDJAR8OZgFxAU8DBgAx
AhQGgAFxARYHogFxAbMHpwEhAtwNrAGBAXwNswHJASYOuAEpAUUPswGBAVILvwGJAZwOxQHRAWgG
rQApAQYGswFpAaQJxQHRAV0DyQHJASYO0AFZAT8CrQDJASYO1wGJAc0OyQA5AlUDBgCBAYALBgCB
AZkJBgDJAXUN7gGBARcB8wFBAh0GBgJJAjYGGwJRAiMGIALJAR8OJwIpAVILUQKZAeUFxQGZATUP
xQGZAT0PVwLBAHUNswEpAbwHoQDJAZgG7gEhAloGiwJ5AfQOWwFhAsUHqwJpAtYOsQJ5AV0DvwLR
AWADyALBACoC6gBxAvIB1QI0AOMO7AI8AOUFSgA8AO8FAwM8AIALBgAIAHQAJAMIAHgAKQMpAKsA
DgYuAAsAFAQuABMAHQQuABsAPAQuACMARQQuACsAdQQuADMAewQuADsARQQuAEMAigQuAEsAdQQu
AFMAqgQuAFsAdQQuAGMAsAQuAGsA2gQuAHMA5wRAAIsAKQNAAIMAMQVDAHsAOgVDAIMAMQVJAKsA
HwZjAHsAOgVjAIMAMQVpAKsAMwaAAIsAKQODAJMAKQODAJsAKQODAHsAOgWJAKsAQAagAIsAKQOp
AIMAPATAAIsAKQPDAJMAKQPDAJsAKQPDALMAKQPDALsAKQPJAIMAPATgAIsAKQPjALsAKQPjAHsA
UwXjAIMAPATpAKsAVAYDAcMAKQMDAVMAdQQjAYMAMQUjAaMArAVDAYMAMQVDAVsAdQRABIMAMQVA
BIsAKQNgBIMAMQVgBIsAKQOABIMAMQWABIsAKQOgBIMAMQWgBIsAKQPABIsAKQPgBIsAKQMABYsA
KQMABYMAMQUgBYsAKQNABYsAKQNABYMAMQVuAO4AbgGFAd8B+AELAi0CNAJcAm0CfwKSApgCpAK4
As0C4AIEAAEABQAFAAYABwAHAAgACgAJAAAADAvsAwAAOgjxAwAAzgr2AwAA8Qv7AwAAdgoABAAA
AwMFBAAAbA0KBAAAZQ4KBAAABQIPBAIABAADAAIABQAFAAIABgAHAAIABwAJAAIACQALAAIACgAN
AAEACwANAAIADAAPAAIADwARAAIAKQATAC4ANQA8AEMA0gLlAvQC+wIEgAAAAQAAAAAAAAAAAAAA
AACjBgAABAAAAAAAAAAAAAAACQPiAAAAAAAEAAAAAAAAAAAAAAAJA80HAAAAAAoAAAAAAAAAAAAA
ABID/AAAAAAADgAAAAAAAAAAAAAAGwPOBgAAAAAOAAAAAAAAAAAAAAAbA2MAAAAAAAQAAAAAAAAA
AAAAAAkDswAAAAAAAAAAAAEAAAADDQAACQAEAAoABAALAAgAAAAQABQAggAAABAATQCCAAAAAABP
AIIA/wDbAv8A/gIAAAAAAENvbnRleHRWYWx1ZWAxAFRocmVhZFNhZmVPYmplY3RQcm92aWRlcmAx
AEludDMyADxNb2R1bGU+AHRyYW5zYWN0aW9uSUQAT2J0YWluUGVyc29uSUQAR2V0UGVyc29uSUQA
TWljcm9zb2Z0LlNxbFNlcnZlci5NYW5hZ2VkRFRTAFQARGlzcG9zZV9fSW5zdGFuY2VfXwBDcmVh
dGVfX0luc3RhbmNlX18AdmFsdWVfXwBTeXN0ZW0uRGF0YQBJbnNlcnRPclVwZGF0ZVVzZXJEYXRh
AFByb2plY3REYXRhAG1zY29ybGliAFN5c3RlbS5EYXRhLk9kYmMATWljcm9zb2Z0LlZpc3VhbEJh
c2ljAFJlYWQAQWRkAE51bWJlck9mUGVyc29uX1VzZXJBZGRlZABudW1iZXJPZkJhZGdlc0FkZGVk
AE51bWJlck9mUGljdHVyZXNBZGRlZABudW1iZXJPZlBlcnNvbnNBZGRlZABOdW1iZXJPZkRlcGFy
dG1lbnRzQWRkZWQAU3luY2hyb25pemVkAGVtcF9pZABiaWQAT2RiY0NvbW1hbmQAU3FsQ29tbWFu
ZABVcGRhdGVCYWRnZVJlY29yZABJbnNlcnRCYWRnZVJlY29yZABJbnNlcnRQZXJzb25SZWNvcmQA
Q3JlYXRlSW5zdGFuY2UAZ2V0X0dldEluc3RhbmNlAGRlZmF1bHRJbnN0YW5jZQBpbnN0YW5jZQBH
ZXRIYXNoQ29kZQBlbXBsb3llZQBnZXRfTWVzc2FnZQBUcmFuc2ZlckJhZGdlAHN0YXR1c01hcFRh
YmxlAElEaXNwb3NhYmxlAEhhc2h0YWJsZQBSdW50aW1lVHlwZUhhbmRsZQBHZXRUeXBlRnJvbUhh
bmRsZQBEYXRlVGltZQBNYWtlSW50VGltZQBNaWNyb3NvZnQuU3FsU2VydmVyLkR0cy5SdW50aW1l
AEdldFR5cGUARmFpbHVyZQBnZXRfQ3VsdHVyZQBzZXRfQ3VsdHVyZQBfcmVzQ3VsdHVyZQBWU1RB
UlRTY3JpcHRPYmplY3RNb2RlbEJhc2UAQXBwbGljYXRpb25CYXNlAEFwcGxpY2F0aW9uU2V0dGlu
Z3NCYXNlAENsb3NlAERpc3Bvc2UAVHJ5UGFyc2UATWFrZUludERhdGUARWRpdG9yQnJvd3NhYmxl
U3RhdGUAQ29tcGlsZXJHZW5lcmF0ZWRBdHRyaWJ1dGUAR3VpZEF0dHJpYnV0ZQBIZWxwS2V5d29y
ZEF0dHJpYnV0ZQBHZW5lcmF0ZWRDb2RlQXR0cmlidXRlAERlYnVnZ2VyTm9uVXNlckNvZGVBdHRy
aWJ1dGUARGVidWdnYWJsZUF0dHJpYnV0ZQBFZGl0b3JCcm93c2FibGVBdHRyaWJ1dGUAQ29tVmlz
aWJsZUF0dHJpYnV0ZQBBc3NlbWJseVRpdGxlQXR0cmlidXRlAFN0YW5kYXJkTW9kdWxlQXR0cmli
dXRlAEhpZGVNb2R1bGVOYW1lQXR0cmlidXRlAEFzc2VtYmx5VHJhZGVtYXJrQXR0cmlidXRlAFRh
cmdldEZyYW1ld29ya0F0dHJpYnV0ZQBEZWJ1Z2dlckhpZGRlbkF0dHJpYnV0ZQBBc3NlbWJseUZp
bGVWZXJzaW9uQXR0cmlidXRlAE15R3JvdXBDb2xsZWN0aW9uQXR0cmlidXRlAEFzc2VtYmx5RGVz
Y3JpcHRpb25BdHRyaWJ1dGUAQ29tcGlsYXRpb25SZWxheGF0aW9uc0F0dHJpYnV0ZQBBc3NlbWJs
eVByb2R1Y3RBdHRyaWJ1dGUAQXNzZW1ibHlDb3B5cmlnaHRBdHRyaWJ1dGUAQ0xTQ29tcGxpYW50
QXR0cmlidXRlAFNTSVNTY3JpcHRUYXNrRW50cnlQb2ludEF0dHJpYnV0ZQBBc3NlbWJseUNvbXBh
bnlBdHRyaWJ1dGUAUnVudGltZUNvbXBhdGliaWxpdHlBdHRyaWJ1dGUAZ2V0X1ZhbHVlAHNldF9W
YWx1ZQBBZGRXaXRoVmFsdWUAQ29udGFpbnNWYWx1ZQBHZXRPYmplY3RWYWx1ZQBJSWYATmV3TGF0
ZUJpbmRpbmcASXNOb3RoaW5nAFN5c3RlbS5SdW50aW1lLlZlcnNpb25pbmcAQ29tcGFyZVN0cmlu
ZwBUb1N0cmluZwBTdWJzdHJpbmcAVHJhY2VMb2cAbXNnAE1hdGgAZ2V0X0xlbmd0aABTdGFydHNX
aXRoAFNUX2ZlZTE5MzA1ODE3MzQ2YzQ4Nzc4ZWFiNGQ0OWM5MjU3LnZicHJvagBNaWNyb3NvZnQu
U3FsU2VydmVyLlNjcmlwdFRhc2sATWljcm9zb2Z0LlNxbFNlcnZlci5EdHMuVGFza3MuU2NyaXB0
VGFzawBHZXRPcmRpbmFsAE1pY3Jvc29mdC5WaXN1YWxCYXNpYy5NeVNlcnZpY2VzLkludGVybmFs
AFNjcmlwdE9iamVjdE1vZGVsAFN5c3RlbS5Db21wb25lbnRNb2RlbABHbG9iYWxFcnJvcmxldmVs
AFNUX2ZlZTE5MzA1ODE3MzQ2YzQ4Nzc4ZWFiNGQ0OWM5MjU3LnZicHJvai5kbGwASXNEQk51bGwA
Z2V0X0l0ZW0AZ2V0X0ZpbGVTeXN0ZW0AVHJpbQBFbnVtAEJvb2xlYW4AU3lzdGVtLkNvbXBvbmVu
dE1vZGVsLkRlc2lnbgBTY3JpcHRNYWluAFRyYW5zZmVyRGl2aXNpb24AZGl2aXNpb24AZ2V0X0Fw
cGxpY2F0aW9uAE15QXBwbGljYXRpb24ASW5mb3JtYXRpb24AU3lzdGVtLkNvbmZpZ3VyYXRpb24A
U3lzdGVtLkdsb2JhbGl6YXRpb24ASW50ZXJhY3Rpb24AU3lzdGVtLlJlZmxlY3Rpb24AT2RiY1Bh
cmFtZXRlckNvbGxlY3Rpb24AU3FsUGFyYW1ldGVyQ29sbGVjdGlvbgBPZGJjQ29ubmVjdGlvbgBw
aWNDb25uZWN0aW9uAEluZm9ybWl4UmVhZENvbm5lY3Rpb24AbGF4aWRDb25uZWN0aW9uAEFjcXVp
cmVDb25uZWN0aW9uAEluZm9ybWl4V3JpdGVDb25uZWN0aW9uAFNxbENvbm5lY3Rpb24ARXhjZXB0
aW9uAFRyYW5zZmVyUGVyc29uAEN1bHR1cmVJbmZvAGJhZGdlVXNlckRhdGFNYXAAcGVyc29uVXNl
ckRhdGFNYXAAdXNlckRhdGFNYXAAQ2xlYXIAQ2hhcgBFeGVjdXRlU2NhbGFyAE9kYmNEYXRhUmVh
ZGVyAFNxbERhdGFSZWFkZXIAbGF4aWRCYWRnZVJlYWRlcgBsYXhpZFBlcnNvblRhYmxlUmVhZGVy
AEV4ZWN1dGVSZWFkZXIAbV9BcHBPYmplY3RQcm92aWRlcgBtX1VzZXJPYmplY3RQcm92aWRlcgBt
X0NvbXB1dGVyT2JqZWN0UHJvdmlkZXIAbV9NeVdlYlNlcnZpY2VzT2JqZWN0UHJvdmlkZXIAbGF4
aWRNYW5hZ2VyAGdldF9SZXNvdXJjZU1hbmFnZXIAQ29ubmVjdGlvbk1hbmFnZXIAaW5mb3JtaXhN
YW5hZ2VyAFRvSW50ZWdlcgBTeXN0ZW0uQ29kZURvbS5Db21waWxlcgBnZXRfVXNlcgBPZGJjUGFy
YW1ldGVyAFNxbFBhcmFtZXRlcgBnZXRfQ29tcHV0ZXIAU2VydmVyQ29tcHV0ZXIATXlDb21wdXRl
cgBfcmVzTWdyAGxvZ0RpcgBDbGVhclByb2plY3RFcnJvcgBTZXRQcm9qZWN0RXJyb3IASUVudW1l
cmF0b3IAR2V0RW51bWVyYXRvcgBJRGljdGlvbmFyeUVudW1lcmF0b3IAQWN0aXZhdG9yAC5jdG9y
AC5jY3RvcgBzdGF0dXNNYXBTdHIAU3lzdGVtLkRpYWdub3N0aWNzAFVwZGF0ZVBlcnNvblJlY29y
ZHMATWljcm9zb2Z0LlZpc3VhbEJhc2ljLkRldmljZXMAZ2V0X1dlYlNlcnZpY2VzAE15V2ViU2Vy
dmljZXMATWljcm9zb2Z0LlZpc3VhbEJhc2ljLkFwcGxpY2F0aW9uU2VydmljZXMAU3lzdGVtLlJ1
bnRpbWUuSW50ZXJvcFNlcnZpY2VzAE1pY3Jvc29mdC5WaXN1YWxCYXNpYy5Db21waWxlclNlcnZp
Y2VzAFN5c3RlbS5SdW50aW1lLkNvbXBpbGVyU2VydmljZXMATWljcm9zb2Z0LlZpc3VhbEJhc2lj
Lk15U2VydmljZXMAU3lzdGVtLlJlc291cmNlcwBTVF9mZWUxOTMwNTgxNzM0NmM0ODc3OGVhYjRk
NDljOTI1Ny52YnByb2ouTXkuUmVzb3VyY2VzAE15UmVzb3VyY2VzAFNUX2ZlZTE5MzA1ODE3MzQ2
YzQ4Nzc4ZWFiNGQ0OWM5MjU3LnZicHJvai5SZXNvdXJjZXMucmVzb3VyY2VzAERlYnVnZ2luZ01v
ZGVzAFVwZGF0ZUluc2VydENhdGVnb3JpZXMAZ2V0X1NldHRpbmdzAEVxdWFscwBDb250YWlucwBD
b252ZXJzaW9ucwBTeXN0ZW0uQ29sbGVjdGlvbnMAZ2V0X0Nvbm5lY3Rpb25zAFJ1bnRpbWVIZWxw
ZXJzAGdldF9QYXJhbWV0ZXJzAE9wZXJhdG9ycwBDb25kaXRpb25hbENvbXBhcmVPYmplY3RMZXNz
AFN1Y2Nlc3MAZ2V0X0R0cwBsYXhDYXRzAFNjcmlwdFJlc3VsdHMAQ29uY2F0AEZvcm1hdABBZGRP
YmplY3QAQ29uY2F0ZW5hdGVPYmplY3QATXlQcm9qZWN0AExhdGVHZXQAU3BsaXQAZ2V0X0RlZmF1
bHQAc2V0X1Rhc2tSZXN1bHQAU3lzdGVtLkRhdGEuU3FsQ2xpZW50AENvbXBvbmVudABnZXRfQ3Vy
cmVudABzcGVjaWFsQ2F0ZWdvcmllc1N0YXJ0U2xvdABBcnJheUxpc3QATW92ZU5leHQAV3JpdGVB
bGxUZXh0AG1fQ29udGV4dABjb19kaXYAZ2V0X05vdwBnZXRfVXRjTm93AFNUX2ZlZTE5MzA1ODE3
MzQ2YzQ4Nzc4ZWFiNGQ0OWM5MjU3LnZicHJvai5NeQBnZXRfS2V5AHNldF9LZXkAQ29udGFpbnNL
ZXkAZ2V0X0Fzc2VtYmx5AEV4ZWN1dGVOb25RdWVyeQBOdW1iZXJPZlBlcnNvbl9DYXRlZ29yeQBU
cmFuc2ZlckJhZGdlQ2F0ZWdvcnkAVHJhbnNmZXJDYXRlZ29yeQBEaWN0aW9uYXJ5RW50cnkATXlT
ZXR0aW5nc1Byb3BlcnR5AEZpbGVTeXN0ZW1Qcm94eQAAMU0AeQAuAFIAZQBzAG8AdQByAGMAZQBz
AC4ATQB5AFIAZQBzAG8AdQByAGMAZQBzAAAXRgA6AFwASgBvAGIATABvAGcAcwBcAACA8UEAQwBU
AEkAVgBFAD0AMAAsAEMATwBOAEYASQBTAEMAQQBUAEUARAA9ADUALABFAFgAUABJAFIARQBEAD0A
NQAsAEkATgBWAEEATABJAEQAPQA1ACwATABPAFMAVAA9ADQALABSAEUAQwBBAEwATAA9ADUALABS
AEUAVABVAFIATgBFAEQAPQA2ACwAVQBOAEMATABBAEkATQBFAEQAPQA1ACwAQwBBAE4AQwBFAEwA
TABFAEQAPQA2ACwAUwBUAE8ATABFAE4APQA0ACwAOQAtADMAMAAtADAAMgAgACAAbgBvAG4AIABG
AFAAPQA1AAEPYwBvAF8AbgBhAG0AZQAAEWQAaQB2AF8AbgBhAG0AZQAAC2MAbwBsAG8AcgAAD2MA
dQBzAHQAbwBtAHMAABNqAG8AYgBfAHQAaQB0AGwAZQAADWQAcgBpAHYAZQByAAAHbABhAHcAAAtn
AGEAdABlAHMAAAlhAHQAYwB0AAAHZABvAGIAAAdzAHMAbgAAC2gAdABfAGYAdAAAC2gAdABfAGkA
bgAADXcAZQBpAGcAaAB0AAAHcwBlAHgAAAllAHkAZQBzAAAJaABhAGkAcgAADWUAdABoAG4AaQBj
AAALZABsAF8AbgBvAAARZABsAF8AcwB0AGEAdABlAAARZABsAF8AZQB4AHAAZAB0AAAPYgBhAGQA
ZwBlAG4AbwAAI0wAYQB4AEIAYQBkAGcAZQBTAHEAbAAuAEwAQQBYAEkARAAAQUEAcQB1AGkAcgBl
AGQAIABjAG8AbgBuAGUAYwB0AGkAbwBuACAAdABvACAATABhAHgAQgBhAGQAZwBlAFMAcQAAD3AA
cAA0AHQAZQBzAHQAADtBAHEAdQBpAHIAZQBkACAAYwBvAG4AbgBlAGMAdABpAG8AbgAgAHQAbwAg
AHAAcAA0AHQAZQBzAHQAAFNFAHgAYwBlAHAAdABpAG8AbgAgAGkAbgAgAGMAbwBuAG4AZQBjAHQA
aQBuAGcAIAB0AG8AIAB0AGgAZQAgAGQAYQB0AGEAYgBhAHMAZQBzACAAAICZUwBlAGwAZQBjAHQA
IABUAE8AUAAgADEAIAAqACAAZgByAG8AbQAgAFQAcgBhAG4AcwBmAGUAcgBfAEMAbwBuAHQAcgBv
AGwAIAB3AGgAZQByAGUAIABFAG4AZABfAHQAcgBhAG4AcwBtAGkAdAAgAGkAcwAgAG4AdQBsAGwA
IABPAFIARABFAFIAIABCAFkAIABJAEQAIAAABUkARAAAaz0APQA9AD0APQA9AD0APQA9AD0APQA9
AD0APQA9AD0APQA9AD0APQA9AD0APQA9AD0APQA9AD0AUAByAG8AYwBlAHMAcwBpAG4AZwAgAHIA
bwB3ACAAdwBpAHQAaAAgAEkARAAgAD0AIAAAgINVAFAARABBAFQARQAgAFQAcgBhAG4AcwBmAGUA
cgBfAEMAbwBuAHQAcgBvAGwAIABTAGUAdAAgAFMAdABhAHIAdABfAFQAcgBhAG4AcwBtAGkAdAA9
AGcAZQB0AEQAYQB0AGUAKAApACAAVwBIAEUAUgBFACAASQBEAD0AQABJAEQAAG9GAGEAaQBsAGUA
ZAAgAHQAbwAgAHUAcABkAGEAdABlACAAcwB0AGEAcgB0AF8AdAByAGEAbgBzAG0AaQB0ACAAZABh
AHQAZQAgAGYAbwByACAAdAByAGEAbgBzAGEAYwB0AGkAbwBuAEkARAAgAAB/VQBQAEQAQQBUAEUA
IABUAHIAYQBuAHMAZgBlAHIAXwBDAG8AbgB0AHIAbwBsACAAUwBlAHQAIABFAG4AZABfAFQAcgBh
AG4AcwBtAGkAdAA9AGcAZQB0AEQAYQB0AGUAKAApACAAVwBIAEUAUgBFACAASQBEAD0AQABJAEQA
AGtGAGEAaQBsAGUAZAAgAHQAbwAgAHUAcABkAGEAdABlACAARQBuAGQAXwB0AHIAYQBuAHMAbQBp
AHQAIABkAGEAdABlACAAZgBvAHIAIAB0AHIAYQBuAHMAYQBjAHQAaQBvAG4ASQBEACAAAICLUwBl
AGwAZQBjAHQAIAAqACAAZgByAG8AbQAgAFQAcgBhAG4AcwBmAGUAcgBfAEMAbwBtAHAAYQBuAHkA
RABpAHYAaQBzAGkAbwBuACAAdwBoAGUAcgBlACAAdAByAGEAbgBzAGYAZQByAEkARAAgAD0AIABA
AHQAcgBhAG4AcwBmAGUAcgBJAEQAABV0AHIAYQBuAHMAZgBlAHIASQBEAAADOQAAC0MATwBfAEkA
RAAADUQASQBWAF8ASQBEAABrcwBlAGwAZQBjAHQAIABpAGQAIABmAHIAbwBtACAAaQBuAGYAbwBy
AG0AaQB4AC4AZABlAHAAYQByAHQAbQBlAG4AdAAgAHcAaABlAHIAZQAgAGQAaQB2AGkAcwBpAG8A
bgAgAD0AIAA/AAARZABpAHYAaQBzAGkAbwBuAAA/RABlAHAAYQByAHQAbQBlAG4AdAAgAGEAbABy
AGUAYQBkAHkAIABlAHgAaQBzAHQAcwAgAEkARAAgAD0AIAAABWkAZAAAgSlVAFAARABBAFQARQAg
AGkAbgBmAG8AcgBtAGkAeAAuAGQAZQBwAGEAcgB0AG0AZQBuAHQAIAAgAFMARQBUACAAZABlAHMA
YwByAGkAcAB0AGkAbwBuAD0APwAsAGwAbwBjAGEAdABpAG8AbgA9AD8ALABtAGEAbgBhAGcAZQBy
AD0APwAsAHAAaABvAG4AZQA9AD8ALAB1AHMAZQByADEAPQA/ACwAdQBzAGUAcgAyAD0APwAsAGYA
YQBjAGkAbABpAHQAeQA9AD8ALAAgAG0AbwBkAGkAZgB5AF8AZABhAHQAZQA9AD8ALAAgAG0AbwBk
AGkAZgB5AF8AdABpAG0AZQA9AD8AIABXAEgARQBSAEUAIABkAGkAdgBpAHMAaQBvAG4APQA/AAAD
IAAACXQAcgBpAG0AABdkAGUAcwBjAHIAaQBwAHQAaQBvAG4AAA9DAE8AXwBOAGEAbQBlAAAFLAAg
AAARRABJAFYAXwBOAGEAbQBlAAARbABvAGMAYQB0AGkAbwBuAAABAA9tAGEAbgBhAGcAZQByAAAL
cABoAG8AbgBlAAALdQBzAGUAcgAxAAAPQwBPAF8ATgBBAE0ARQAAC3UAcwBlAHIAMgAAEUQASQBW
AF8ATgBBAE0ARQAAEWYAYQBjAGkAbABpAHQAeQAAF20AbwBkAGkAZgB5AF8AZABhAHQAZQAAEXkA
eQB5AHkATQBNAGQAZAAAF20AbwBkAGkAZgB5AF8AdABpAG0AZQAADUgASABtAG0AcwBzAABdVwBh
AHIAbgBpAG4AZwA6ACAATgBvACAAcgBvAHcAcwAgAHUAcABkAGEAdABlAGQAIABiAHkAIABVAHAA
ZABhAHQAZQAgAGQAZQBwAGEAcgB0AG0AZQBuAHQAIAAAJ0QAZQBwAGEAcgB0AG0AZQBuAHQAIABV
AHAAZABhAHQAZQBkACAAAGdDAGEAcwBpAF8ARABlAHAAYQByAHQAbQBlAG4AdAA6ACAARQB4AGMA
ZQBwAHQAaQBvAG4AIAB1AHAAZABhAHQAaQBuAGcAIABuAGUAdwAgAGQAZQBwAGEAcgB0AG0AZQBu
AHQAIAAAAzoAAGFDAGEAcwBpAF8ARABlAHAAYQByAHQAbQBlAG4AdAA6AEUAeABjAGUAcAB0AGkA
bwBuACAAaQBuACAAUgBlAGEAZABpAG4AZwAgAGQAZQBwAGEAcgB0AG0AZQBuAHQAIAAAgTNJAE4A
UwBFAFIAVAAgAEkATgBUAE8AIABpAG4AZgBvAHIAbQBpAHgALgBkAGUAcABhAHIAdABtAGUAbgB0
ACAAKABkAGUAcwBjAHIAaQBwAHQAaQBvAG4ALABkAGkAdgBpAHMAaQBvAG4ALABsAG8AYwBhAHQA
aQBvAG4ALABtAGEAbgBhAGcAZQByACwAcABoAG8AbgBlACwAdQBzAGUAcgAxACwAdQBzAGUAcgAy
ACwAZgBhAGMAaQBsAGkAdAB5ACwAIABtAG8AZABpAGYAeQBfAGQAYQB0AGUALAAgAG0AbwBkAGkA
ZgB5AF8AdABpAG0AZQApACAAVgBBAEwAVQBFAFMAIAAoAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAs
AD8ALAA/ACwAPwAsAD8AKQAAS1cAYQByAG4AaQBuAGcAOgAgAEYAYQBpAGwAZQBkACAAdABvACAA
YwByAGUAYQB0AGUAIABkAGUAcABhAHIAdABtAGUAbgB0ACAAAC9OAGUAdwAgAGQAZQBwAGEAcgB0
AG0AZQBuAHQAIABjAHIAZQBhAHQAZQBkACAAAGdDAGEAcwBpAF8ARABlAHAAYQByAHQAbQBlAG4A
dAA6ACAARQB4AGMAZQBwAHQAaQBvAG4AIABjAHIAZQBhAHQAaQBuAGcAIABuAGUAdwAgAGQAZQBw
AGEAcgB0AG0AZQBuAHQAIAAAfVMAZQBsAGUAYwB0ACAAKgAgAGYAcgBvAG0AIABUAHIAYQBuAHMA
ZgBlAHIAXwBDAGEAdABlAGcAbwByAHkAIAB3AGgAZQByAGUAIAB0AHIAYQBuAHMAZgBlAHIASQBE
ACAAPQAgAEAAdAByAGEAbgBzAGYAZQByAEkARAAAgJFVAFAARABBAFQARQAgAEMAYQB0AGUAZwBv
AHIAeQAgAFMARQBUACAAZABlAHMAYwByAGkAcAB0AGkAbwBuAD0APwAsAG0AbwBkAGkAZgB5AF8A
ZABhAHQAZQA9AD8ALABtAG8AZABpAGYAeQBfAHQAaQBtAGUAPQA/ACAAVwBIAEUAUgBFACAAaQBk
AD0APwAADWQAZQBzAGMAcgBwAAALQwBhAHQASQBEAACA70kATgBTAEUAUgBUACAASQBOAFQATwAg
AEMAYQB0AGUAZwBvAHIAeQAgACgAaQBkACwAZABlAHMAYwByAGkAcAB0AGkAbwBuACwAcABlAHIA
bQBpAHMAcwBpAG8AbgBfAGcAcgBwACwAbQAyAG0AcgBfAHQAeQBwAGUALABmAGEAYwBpAGwAaQB0
AHkALABtAG8AZABpAGYAeQBfAGQAYQB0AGUALABtAG8AZABpAGYAeQBfAHQAaQBtAGUAKQAgAFYA
QQBMAFUARQBTACAAKAA/ACwAPwAsAC0AMQAsADAALAA/ACwAPwAsAD8AKQABK04AZQB3ACAAYwBh
AHQAZQBnAG8AcgB5ACAAYwByAGUAYQB0AGUAZAAgAAALQwBhAHQASQBkAAA5QwBhAHQAZQBnAG8A
cgB5ACAAcgBlAGMAbwByAGQAIAB1AHAAZABhAHQAZQBkAC4AIABpAGQAPQAAP0UAeABjAGUAcAB0
AGkAbwBuACAAYwByAGUAYQB0AGkAbgBnACAAQwBhAHQAZQBnAG8AcgB5ACAASQBEAD0AAIHRUwBF
AEwARQBDAFQAIABwAGMALgBpAGQAIABJAEQALAAgAHAAYwAuAGMAYQB0AGUAZwBvAHIAeQBfAGkA
ZAAsACAAcABjAC4AcwBsAG8AdABfAG4AdQBtAGIAZQByACwAIABwAC4AaQBkACAAcABlAHIAcwBv
AG4AXwBpAGQAIABmAHIAbwBtACAAcABlAHIAcwBvAG4AIABwACAAaQBuAG4AZQByACAAagBvAGkA
bgAgAGQAZQBwAGEAcgB0AG0AZQBuAHQAIABkACAAbwBuACAAcAAuAGQAZQBwAGEAcgB0AG0AZQBu
AHQAPQBkAC4AaQBkACAAbABlAGYAdAAgAGoAbwBpAG4AIABwAGUAcgBzAG8AbgBfAGMAYQB0AGUA
ZwBvAHIAeQAgAHAAYwAgAG8AbgAgAHAAYwAuAHAAZQByAHMAbwBuAF8AaQBkAD0AcAAuAGkAZAAg
AFcAaABlAHIAZQAgAHAALgBlAG0AcABsAG8AeQBlAGUAPQA/ACAAYQBuAGQAIABkAC4AZABpAHYA
aQBzAGkAbwBuAD0APwAgAE8AcgBkAGUAcgAgAGIAeQAgAHAAYwAuAGMAYQB0AGUAZwBvAHIAeQBf
AGkAZAAAEWUAbQBwAGwAbwB5AGUAZQAAE3AAZQByAHMAbwBuAF8AaQBkAAAXYwBhAHQAZQBnAG8A
cgB5AF8AaQBkAAAXcwBsAG8AdABfAG4AdQBtAGIAZQByAAB/RABFAEwARQBUAEUAIABGAHIAbwBt
ACAAcABlAHIAcwBvAG4AXwBjAGEAdABlAGcAbwByAHkAIAB3AGgAZQByAGUAIABwAGUAcgBzAG8A
bgBfAGkAZAA9AD8AIABhAG4AZAAgAGMAYQB0AGUAZwBvAHIAeQBfAGkAZAA9AD8AAFVQAGUAcgBz
AG8AbgAgAEMAYQB0AGUAZwBvAHIAeQAgAHsAMAB9ACAARABlAGwAZQB0AGUAZAAgAGYAbwByACAA
cABlAHIAcwBvAG4AIAB7ADEAfQAAAy4AAFNTAEUATABFAEMAVAAgAE0AQQBYACgASQBEACkAIABN
AGEAeABJAEQAIABmAHIAbwBtACAAcABlAHIAcwBvAG4AXwBjAGEAdABlAGcAbwByAHkAAIEBSQBO
AFMARQBSAFQAIABJAG4AdABvACAAcABlAHIAcwBvAG4AXwBjAGEAdABlAGcAbwByAHkAIAAoAGkA
ZAAsACAAcABlAHIAcwBvAG4AXwBpAGQALAAgAGMAYQB0AGUAZwBvAHIAeQBfAGkAZAAsACAAcwBs
AG8AdABfAG4AdQBtAGIAZQByACwAIABmAGEAYwBpAGwAaQB0AHkALAAgAG0AbwBkAGkAZgB5AF8A
ZABhAHQAZQAsACAAbQBvAGQAaQBmAHkAXwB0AGkAbQBlACkAIABWAEEATABVAEUAUwAgACgAPwAs
AD8ALAA/ACwAPwAsAD8ALAA/ACwAPwApAACAn0EAZABkAGkAbgBnACAAUABlAHIAcwBvAG4AXwBD
AGEAdABlAGcAbwByAHkAIABpAGQAPQB7ADAAfQAsACAAcABlAHIAcwBvAG4AXwBpAGQAPQB7ADEA
fQAsACAAcwBsAG8AdABfAG4AdQBtAGIAZQByAD0AewAyAH0ALAAgAGMAYQB0AGUAZwBvAHIAeQBf
AGkAZAA9AHsAMwB9ACAAAFtGAGEAaQBsAGUAZAAgAHQAbwAgAGkAbgBzAGUAcgB0ACAAQwBhAHQA
ZQBnAG8AcgB5ACAAewAwAH0AIABmAG8AcgAgAHAAZQByAHMAbwBuACAAewAxAH0AIAAAT1AAZQBy
AHMAbwBuACAAQwBhAHQAZQBnAG8AcgB5ACAAewAwAH0AIABBAGQAZABlAGQAIAB0AG8AIABwAGUA
cgBzAG8AbgAgAHsAMQB9AABrRQB4AGMAZQBwAHQAaQBvAG4AIABJAG4AcwBlAHIAdABpAG4AZwAg
AEMAYQB0AGUAZwBvAHIAeQAgAHsAMAB9ACAAZgBvAHIAIABwAGUAcgBzAG8AbgAgAHsAMQB9ADoA
IAB7ADIAfQAgAACBF1MAZQBsAGUAYwB0ACAAZABpAHMAdABpAG4AYwB0ACAAZQBtAHAAXwBpAGQA
LAAgAGMAbwBfAGQAaQB2ACwAIABjAGEAdABlAGcAbwByAHkAXwBpAGQAIABmAHIAbwBtACAAVABy
AGEAbgBzAGYAZQByAF8AQgBhAGQAZwBlAEMAYQB0AGUAZwBvAHIAeQAgAHcAaABlAHIAZQAgAHQA
cgBhAG4AcwBmAGUAcgBJAEQAIAA9ACAAQAB0AHIAYQBuAHMAZgBlAHIASQBEACAAbwByAGQAZQBy
ACAAYgB5ACAAZQBtAHAAXwBpAGQALAAgAGMAbwBfAGQAaQB2ACwAIABjAGEAdABlAGcAbwByAHkA
XwBpAGQAAA1lAG0AcABfAGkAZAAADWMAbwBfAGQAaQB2AAAXQwBhAHQAZQBnAG8AcgB5AF8ASQBE
AACBvXUAcABkAGEAdABlACAAaQBuAGYAbwByAG0AaQB4AC4AcABlAHIAcwBvAG4AIABzAGUAdAAg
AHAAaQBuAD0APwAsACAAZgBpAHIAcwB0AF8AbgBhAG0AZQA9AD8ALAAgAGwAYQBzAHQAXwBuAGEA
bQBlAD0APwAsACAAaQBuAGkAdABpAGEAbABzAD0APwAsACAAdABpAHQAbABlACAAPQA/ACwAIABh
AGQAZAByAGUAcwBzADEAPQA/ACwAIABhAGQAZAByAGUAcwBzADIAPQA/ACwAIABhAGQAZAByAGUA
cwBzADMAPQA/ACwAIABhAGQAZAByAGUAcwBzADQAPQA/ACwAIABhAGQAZAByAGUAcwBzADUAPQA/
ACwAIABwAGgAbwBuAGUAPQA/ACAALAAgAG0AbwBkAGkAZgB5AF8AZABhAHQAZQA9AD8AIAAsAG0A
bwBkAGkAZgB5AF8AdABpAG0AZQA9AD8AIAB3AGgAZQByAGUAIABwAGUAcgBzAG8AbgAuAHMAdABh
AHQAdQBzACAAPQAgADAAIABBAG4AZAAgAGUAbQBwAGwAbwB5AGUAZQAgAD0AIAA/AAAHcABpAG4A
ABVmAGkAcgBzAHQAXwBuAGEAbQBlAAALZgBuAGEAbQBlAAATbABhAHMAdABfAG4AYQBtAGUAAAts
AG4AYQBtAGUAABFpAG4AaQB0AGkAYQBsAHMAAA1tAGkAZABkAGwAZQAAC3QAaQB0AGwAZQAAEWEA
ZABkAHIAZQBzAHMAMQAADXMAdAByAGUAZQB0AAARYQBkAGQAcgBlAHMAcwAyAAALYQBwAHQAbgBv
AAARYQBkAGQAcgBlAHMAcwAzAAAJYwBpAHQAeQAAEWEAZABkAHIAZQBzAHMANAAAC3MAdABhAHQA
ZQAAEWEAZABkAHIAZQBzAHMANQAAB3oAaQBwAAAPYwBvAHUAbgB0AHIAeQAADXcAcABoAG8AbgBl
AABRRQB4AGMAZQBwAHQAaQBvAG4AIABpAG4AIAB1AHAAZABhAHQAaQBuAGcAIABwAGUAcgBzAG8A
bgAgAHIAZQBjAG8AcgBkACAAZgBvAHIAIAAAVVMARQBMAEUAQwBUACAAaQBkACAAZgByAG8AbQAg
AGQAZQBwAGEAcgB0AG0AZQBuAHQAIAB3AGgAZQByAGUAIABkAGkAdgBpAHMAaQBvAG4APQA/AACA
n0QAZQBwAGEAcgB0AG0AZQBuAHQAIAB7ADAAfQAgAEQAbwBlAHMAIABuAG8AdAAgAGUAeABpAHMA
dAAuACAARgBhAGkAbABlAGQAIAB0AG8AIABjAHIAZQBhAHQAZQAgAHAAZQByAHMAbwBuACAAewAx
AH0AIAB3AGkAdABoACAAdABoAGkAcwAgAGQAZQBwAGEAcgB0AG0AZQBuAHQAAIP9SQBOAFMARQBS
AFQAIABJAE4AVABPACAAaQBuAGYAbwByAG0AaQB4AC4AcABlAHIAcwBvAG4AIAAoAHAAaQBuACwA
IABzAHQAYQB0AHUAcwAsACAAdAB5AHAAZQAsACAAcABlAHIAcwBvAG4AXwBrAHAAXwByAGUAcwBw
ACwAIABwAGUAcgBzAG8AbgBfAHQAcgBhAGMAZQAsACAAcABlAHIAcwBvAG4AXwB0AHIAYQBjAGUA
XwBhAGwAYQByAG0ALAAgAGUAbQBwAGwAbwB5AGUAZQAsACAAZABlAHAAYQByAHQAbQBlAG4AdAAs
ACAAZgBpAHIAcwB0AF8AbgBhAG0AZQAsAGwAYQBzAHQAXwBuAGEAbQBlACwAIABpAG4AaQB0AGkA
YQBsAHMALAAgAHQAaQB0AGwAZQAsACAAYQBkAGQAcgBlAHMAcwAxACwAIABhAGQAZAByAGUAcwBz
ADIALAAgAGEAZABkAHIAZQBzAHMAMwAsACAAYQBkAGQAcgBlAHMAcwA0ACwAYQBkAGQAcgBlAHMA
cwA1ACwAIABwAGgAbwBuAGUALAAgAHAAaABvAG4AZQAyACwAIAAJAHIAZQBpAHMAcwB1AGUAXwBj
AG4AdAAsACAAYQBwAGIALAAgAHIAZQBhAGQAZQByACwAIABhAGMAYwBlAHMAcwBfAGQAYQB0AGUA
LABhAGMAYwBlAHMAcwBfAHQAaQBtAGUALAAgAGEAYwBjAGUAcwBzAF8AdAB6ACwAIABhAGMAdABp
AHYAZQBfAGQAYQB0AGUALAAgAGEAYwB0AGkAdgBlAF8AdABpAG0AZQAsACAAYQBjAHQAaQB2AGUA
XwBjAG8AbgB0AGUAeAB0ACwAIABkAGUAYQBjAHQAaQB2AGUAXwBkAGEAdABlACwAIABkAGUAYQBj
AHQAaQB2AGUAXwB0AGkAbQBlACwAIABkAGUAYQBjAHQAaQB2AGUAXwBjAG8AbgB0AGUAeAB0ACwA
IABmAG8AcgBjAGUAXwBkAG8AdwBuAGwAbwBhAGQALAAgAGYAYQBjAGkAbABpAHQAeQAsACAAbQBv
AGQAaQBmAHkAXwBkAGEAdABlACwAIABtAG8AZABpAGYAeQBfAHQAaQBtAGUAKQAgAHYAYQBsAHUA
ZQBzACAAKAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAIAA/ACwAPwAsAD8ALAA/ACwAbgB1
AGwAbAAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8A
LAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACkAAA1zAHQAYQB0AHUAcwAA
CXQAeQBwAGUAAB1wAGUAcgBzAG8AbgBfAGsAcABfAHIAZQBzAHAAABlwAGUAcgBzAG8AbgBfAHQA
cgBhAGMAZQAAJXAAZQByAHMAbwBuAF8AdAByAGEAYwBlAF8AYQBsAGEAcgBtAAAVZABlAHAAYQBy
AHQAbQBlAG4AdAAADXAAaABvAG4AZQAyAAAXcgBlAGkAcwBzAHUAZQBfAGMAbgB0AAAHYQBwAGIA
AA1yAGUAYQBkAGUAcgAAF2EAYwBjAGUAcwBzAF8AZABhAHQAZQAAF2EAYwBjAGUAcwBzAF8AdABp
AG0AZQAAE2EAYwBjAGUAcwBzAF8AdAB6AAAXYQBjAHQAaQB2AGUAXwBkAGEAdABlAAAXYQBjAHQA
aQB2AGUAXwB0AGkAbQBlAAAdYQBjAHQAaQB2AGUAXwBjAG8AbgB0AGUAeAB0AAAbZABlAGEAYwB0
AGkAdgBlAF8AZABhAHQAZQAAG2QAZQBhAGMAdABpAHYAZQBfAHQAaQBtAGUAACFkAGUAYQBjAHQA
aQB2AGUAXwBjAG8AbgB0AGUAeAB0AAAdZgBvAHIAYwBlAF8AZABvAHcAbgBsAG8AYQBkAABDMAAg
AFIAbwB3AHMAIABJAG4AcwBlAHIAdABlAGQAIABmAG8AcgAgAE4AZQB3ACAAUABlAHIAcwBvAG4A
IAA9ACAAACdOAGUAdwAgAFAAZQByAHMAbwBuACAAYwByAGUAYQB0AGUAZAAgAAA9RQB4AGMAZQBw
AHQAaQBvAG4AIABpAG4AIABpAG4AcwBlAHIAdABpAG4AZwAgAFAAZQByAHMAbwBuACAAAHlTAGUA
bABlAGMAdAAgACoAIABmAHIAbwBtACAAVAByAGEAbgBzAGYAZQByAF8AUABlAHIAcwBvAG4AIAB3
AGgAZQByAGUAIAB0AHIAYQBuAHMAZgBlAHIASQBEACAAPQAgAEAAdAByAGEAbgBzAGYAZQByAEkA
RAAAF1AAZQByAHMAbwBuACAASQBEACAAPQAABS4AIAAAISAAUgBlAGMAbwByAGQAcwAgAFUAcABk
AGEAdABlAGQAAE1TAGUAbABlAGMAdAAgAGkAZAAgAGYAcgBvAG0AIABwAGUAcgBzAG8AbgAgAHcA
aABlAHIAZQAgAGUAbQBwAGwAbwB5AGUAZQA9AD8AAEdGAGEAaQBsAGUAZAAgAHQAbwAgAHIAZQB0
AHIAaQBlAHYAZQAgAEkAbgBzAGUAcgB0AGUAZAAgAFAAZQByAHMAbwBuACAAAAlJAHQAZQBtAACA
zVUAcABkAGEAdABlACAAcABlAHIAcwBvAG4AXwB1AHMAZQByACAAcwBlAHQAIABkAGUAcwBjAHIA
aQBwAHQAaQBvAG4APQA/ACwAIABtAG8AZABpAGYAeQBfAGQAYQB0AGUAPQA/ACwAIABtAG8AZABp
AGYAeQBfAHQAaQBtAGUAPQA/ACAAVwBIAEUAUgBFACAAcABlAHIAcwBvAG4AXwBpAGQAPQA/ACAA
QQBOAEQAIABzAGwAbwB0AF8AbgB1AG0AYgBlAHIAPQA/AACA+UkAbgBzAGUAcgB0ACAASQBuAHQA
bwAgAHAAZQByAHMAbwBuAF8AdQBzAGUAcgAgACgAaQBkACwAIABkAGUAcwBjAHIAaQBwAHQAaQBv
AG4ALAAgAHAAZQByAHMAbwBuAF8AaQBkACwAIABzAGwAbwB0AF8AbgB1AG0AYgBlAHIALAAgAGYA
YQBjAGkAbABpAHQAeQAsACAAbQBvAGQAaQBmAHkAXwBkAGEAdABlACwAIABtAG8AZABpAGYAeQBf
AHQAaQBtAGUAKQAgAFYAQQBMAFUARQBTACAAKAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACkA
AICNRQB4AGMAZQBwAHQAaQBvAG4AIABpAG4AIABzAGUAdAB0AGkAbgBnACAAdQBzAGUAcgAgAHYA
YQBsAHUAZQAgAGUAbQBwAGwAbwB5AGUAZQA9AHsAMAB9ACwAIABzAGwAbwB0AD0AewAxAH0ALAAg
AHYAYQBsAHUAZQA9AHsAMgB9ADoAewAzAH0AIAAADUQAaQB2AF8ASQBEAAALQwBPAEwATwBSAACB
MXUAcABkAGEAdABlACAAaQBuAGYAbwByAG0AaQB4AC4AYgBhAGQAZwBlACAAcwBlAHQAIABkAGUA
cwBjAHIAaQBwAHQAaQBvAG4APQA/ACwAIABwAGUAcgBzAG8AbgBfAGkAZAA9AD8ALAAgAHIAZQB0
AHUAcgBuAF8AZABhAHQAZQA9ACAAPwAsACAAcgBlAHQAdQByAG4AXwB0AGkAbQBlAD0AIAA/ACwA
cgBlAHQAdQByAG4AXwB0AHoAPQAgAD8ALABzAHQAYQB0AHUAcwA9AD8ALAAgAG0AbwBkAGkAZgB5
AF8AZABhAHQAZQA9ACAAPwAsACAAbQBvAGQAaQBmAHkAXwB0AGkAbQBlAD0AIAA/ACAAdwBoAGUA
cgBlACAAYgBpAGQAIAA9ACAAPwAAF3IAZQB0AHUAcgBuAF8AZABhAHQAZQAAE1IARQBUAFUAUgBO
AF8ARABUAAAXcgBlAHQAdQByAG4AXwB0AGkAbQBlAAATcgBlAHQAdQByAG4AXwB0AHoAAAdiAGkA
ZAAAP0YAYQBpAGwAZQBkACAAdABvACAAVQBwAGQAYQB0AGUAIABiAGEAZABnAGUAIABuAHUAbQBi
AGUAcgA9ACAAABMsACAAUwB0AGEAdAB1AHMAPQAALVUAcABkAGEAdABlAGQAIABiAGEAZABnAGUA
IABuAHUAbQBiAGUAcgA9ACAAAEFFAHgAYwBlAHAAdABpAG8AbgAgAHUAcABkAGEAdABpAG4AZwAg
AGIAYQBkAGcAZQAgAG4AdQBtAGIAZQByAD0AAIOdSQBOAFMARQBSAFQAIABJAE4AVABPACAAaQBu
AGYAbwByAG0AaQB4AC4AYgBhAGQAZwBlACAAKABkAGUAcwBjAHIAaQBwAHQAaQBvAG4ALAAgAGIA
aQBkACwAIABzAHQAYQB0AHUAcwAsACAAYgBhAGQAZwBlAF8AdABvAHUAcgAsACAAYgBhAGQAZwBl
AF8AdABlAG0AcAAsACAAcABlAHIAcwBvAG4AXwBpAGQALAAgAHIAZQBhAGQAZQByACwAIABhAGMA
YwBlAHMAcwBfAGQAYQB0AGUALAAgAGEAYwBjAGUAcwBzAF8AdABpAG0AZQAsACAAYQBjAGMAZQBz
AHMAXwB0AHoALAAgAGkAcwBzAHUAZQBfAGQAYQB0AGUALAAgAGkAcwBzAHUAZQBfAHQAaQBtAGUA
LABpAHMAcwB1AGUAXwBjAG8AbgB0AGUAeAB0ACwAIABlAHgAcABpAHIAZQBkAF8AZABhAHQAZQAs
ACAAZQB4AHAAaQByAGUAZABfAHQAaQBtAGUALABlAHgAcABpAHIAZQBkAF8AYwBvAG4AdABlAHgA
dAAsACAAcgBlAHQAdQByAG4AXwBkAGEAdABlACwAIAByAGUAdAB1AHIAbgBfAHQAaQBtAGUALABy
AGUAdAB1AHIAbgBfAHQAegAsACAAdQBzAGEAZwBlAF8AYwBvAHUAbgB0ACwAdQBzAGEAZwBlAF8A
ZQB4AGgAYQB1AHMAdABlAGQALAAgAHQAbwB1AHIAXwBiAGEAZABnAGUALAAgAGIAaQBkAF8AZgBv
AHIAbQBhAHQAXwBpAGQALAAgAHIAZQBpAHMAcwB1AGUAXwBjAG4AdAAsAHIAZQBwAHIAaQBuAHQA
XwBjAG4AdAAsACAAdQBuAGkAcQB1AGUAXwBpAGQALAAgAGIAYQBkAGcAZQBfAGQAZQBzAGkAZwBu
ACwAIABmAGEAYwBpAGwAaQB0AHkALAAgAG0AbwBkAGkAZgB5AF8AZABhAHQAZQAsACAAbQBvAGQA
aQBmAHkAXwB0AGkAbQBlACkAIAB2AGEAbAB1AGUAcwAgACgAPwAsAD8ALAA/ACwAPwAsAD8ALAA/
ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8A
LAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACwAPwAsAD8ALAA/ACkAABViAGEAZABnAGUAXwB0
AG8AdQByAAAVYgBhAGQAZwBlAF8AdABlAG0AcAAAFWkAcwBzAHUAZQBfAGQAYQB0AGUAABFpAHMA
cwB1AGUAXwBkAHQAABVpAHMAcwB1AGUAXwB0AGkAbQBlAAAbaQBzAHMAdQBlAF8AYwBvAG4AdABl
AHgAdAAAGWUAeABwAGkAcgBlAGQAXwBkAGEAdABlAAANZQB4AHAAXwBkAHQAABllAHgAcABpAHIA
ZQBkAF8AdABpAG0AZQAAH2UAeABwAGkAcgBlAGQAXwBjAG8AbgB0AGUAeAB0AAAXdQBzAGEAZwBl
AF8AYwBvAHUAbgB0AAAfdQBzAGEAZwBlAF8AZQB4AGgAYQB1AHMAdABlAGQAABV0AG8AdQByAF8A
YgBhAGQAZwBlAAAbYgBpAGQAXwBmAG8AcgBtAGEAdABfAGkAZAAACzAAMAAxADAAMQAABTAAMAAA
F3IAZQBwAHIAaQBuAHQAXwBjAG4AdAAAE3UAbgBpAHEAdQBlAF8AaQBkAAAPQgBhAGQAZwBlAE4A
bwAAGWIAYQBkAGcAZQBfAGQAZQBzAGkAZwBuAAA3RgBhAGkAbABlAGQAIAB0AG8AIABjAHIAZQBh
AHQAZQAgAG4AZQB3ACAAYgBhAGQAZwBlACAAACVDAHIAZQBhAHQAZQBkACAAbgBlAHcAIABiAGEA
ZABnAGUAIAAAO0UAeABjAGUAcAB0AGkAbwBuACAAaQBuACAAaQBuAHMAZQByAHQAaQBuAGcAIABi
AGEAZABnAGUAIAAAd1MAZQBsAGUAYwB0ACAAKgAgAGYAcgBvAG0AIABUAHIAYQBuAHMAZgBlAHIA
XwBCAGEAZABnAGUAIAB3AGgAZQByAGUAIAB0AHIAYQBuAHMAZgBlAHIASQBEACAAPQAgAEAAdABy
AGEAbgBzAGYAZQByAEkARAAAD0IAYQBkAGcAZQBuAG8AAFdzAGUAbABlAGMAdAAgAGkAZAAgAGYA
cgBvAG0AIABpAG4AZgBvAHIAbQBpAHgALgBiAGEAZABnAGUAIAB3AGgAZQByAGUAIABiAGkAZAA9
ACAAPwAgAAANQwBhAHIAZABuAG8AAAMwAAALMAAwADUAMgAwAACA21MARQBMAEUAQwBUACAAcAAu
AGkAZAAgAGkAZAAgAEYAUgBPAE0AIABQAGUAcgBzAG8AbgAgAHAAIABJAG4AbgBlAHIAIABKAG8A
aQBuACAARABlAHAAYQByAHQAbQBlAG4AdAAgAGQAIABvAG4AIABwAC4AZABlAHAAYQByAHQAbQBl
AG4AdAA9AGQALgBpAGQAIABXAGgAZQByAGUAIABwAC4ARQBtAHAAbABvAHkAZQBlAD0APwAgAEEA
bgBkACAAZAAuAGQAaQB2AGkAcwBpAG8AbgA9AD8AABFFAG0AcABsAG8AeQBlAGUAABFEAGkAdgBp
AHMAaQBvAG4AAFNTAGUAbABlAGMAdAAgACoAIABmAHIAbwBtACAAcABlAHIAcwBvAG4AIAB3AGgA
ZQByAGUAIABlAG0AcABfAGkAZAA9AEAAZQBtAHAAXwBpAGQAADtOAG8AIABQAGUAcgBzAG8AbgAg
AHIAZQBjAG8AcgBkACAAZgBvAHIAIABlAG0AcABfAGkAZAA9ACAAABtBAEMAQQBNAFMAVAByAGEA
bgBzAGYAZQByAAAVeQB5AHkAeQAtAE0ATQAtAGQAZAABCS4AbABvAGcAACd5AHkAeQB5AC0ATQBN
AC0AZABkACAASABIADoAbQBtADoAcwBzAAEFIABMAAAFOgAgAAAFDQAKAAAAAPWegk964R9IhF98
7NL4sA0ABCABAQgDIAABBSABARERBCABAQ4EIAEBAgUgAgEODgUgAQERRQcgBAEODg4OBhUSKAES
DAYVEigBEggGFRIoARJlBhUSKAESJAQgABMABwABEm0RgNUFIAASgNkHIAIBDhKA2QgAARKA3RKA
3RIHCBKAqQgdDggdDhKArRKAsQgGIAEdDh0DBSACARwcBSAAEoDtBSAAEoDxBiABEoCZHAQgARwc
BgABARKArQMgAA4FAAIODg4DAAABByACAQ4SgJ0FIAASgLEDIAACBCABHA4EAAEIHAQAAQ4IBSAA
EoEBByACEoEFDhwDIAAIJwcREoCpEoCxDhKAtRKAuQ4SgLUODg4SgLUODhGAvRKArRKArRKArQYA
Aw4ODg4HIAIBDhKAoQUgABKBCQcgAhKBDQ4cBSAAEoC5BQACHBwcBAABDhwQAAccHBJtDh0cHQ4d
Em0dAgUgAg4ICAUAABGAvQQgAQ4OBwAEDg4ODg4RBwUSgKkSgLESgLURgL0SgK0EAAEcHBwHDA4S
gLUSgLkSgJUICAgSgMUIEoC1EYC9EoCtBCABCA4EIAECCAYAAwIcHAIEIAECHAYAAw4OHBwFIAAS
gMUDIAAcBgACAg4QCAYAAg4OHRwHAAQODhwcHA4HBhKAqQ4OEoCxEoDBAgQgAQIOBCABCBwNBwUS
gLUOCBGAvRKArQQGEoEhDwcHCAgSgLUOCBGAvRKArQQAAQIcBgADHAIcHAUAAQ4dDgYHAhKAsQgc
BwwSgLUOCBKAyRGAzRGAzQgOHRwdAhGAvRKArQUgABKAyQQgAQEcEAcIDggODhKAtQgRgL0SgK0R
BwkOCA4OEoC1CAgRgL0SgK0LBwUSgLEODhKAuQIGAAMIDg4CBQcDCA4ICwcFCAgcEoCpEoCxBgcC
DhGAvQUgABKBNQYgAwEODgIGBwIcEYC9CAACAg4QEYC9BAABCA4EBwEeAAIeAAUQAQAeAAQKAR4A
BAcBEwAGFRIoARMABwYVEnEBEwAGFRJxARMAAhMABAoBEwAFIAEBEwAIt3pcVhk04IkIsD9ffxHV
CjoIiYRdzYCAzJEEAAAAAAQBAAAABwYVEigBEgwHBhUSKAESCAcGFRIoARJlBwYVEigBEiQDBhJ5
AwYSfQMGEhwCBg4CBggEBhKAlQQGEoCZBAYSgJ0EBhKAoQMGESwEAAASDAQAABIIBAAAEmUEAAAS
JAQAABJ5BAAAEn0FAAEBEn0EAAASHAggAwESgMEODgYgAQgSgLEHIAIIEoCxDgkgAgESgJUSgLEH
IAIIDhKAsQUgAggODgUgAgEIDgQgABJtBxABAR4AHgAHMAEBARAeAAQIABIMBAgAEggECAASZQQI
ABIkBAgAEnkECAASfQQIABIcBCgAEwAIAQAIAAAAAAAeAQABAFQCFldyYXBOb25FeGNlcHRpb25U
aHJvd3MBCAEAAgAAAAAALwEAKlNUX2ZlZTE5MzA1ODE3MzQ2YzQ4Nzc4ZWFiNGQ0OWM5MjU3LnZi
cHJvagAABQEAAAAADgEACU1pY3Jvc29mdAAAHwEAGkNvcHlyaWdodCBAIE1pY3Jvc29mdCAyMDEw
AAAFAQABAAApAQAkYzIwY2MyNGUtODAwOC00ZjI3LWI4YWEtOTIwZjk5MWJhZGEzAAAMAQAHMS4w
LjAuMAAASQEAGi5ORVRGcmFtZXdvcmssVmVyc2lvbj12NC41AQBUDhRGcmFtZXdvcmtEaXNwbGF5
TmFtZRIuTkVUIEZyYW1ld29yayA0LjUIAQABAAAAAAAYAQAKTXlUZW1wbGF0ZQgxMS4wLjAuMAAA
WAEAS01pY3Jvc29mdC5WaXN1YWxTdHVkaW8uRWRpdG9ycy5TZXR0aW5nc0Rlc2lnbmVyLlNldHRp
bmdzU2luZ2xlRmlsZUdlbmVyYXRvcgc5LjAuMC4wAABhAQA0U3lzdGVtLldlYi5TZXJ2aWNlcy5Q
cm90b2NvbHMuU29hcEh0dHBDbGllbnRQcm90b2NvbBJDcmVhdGVfX0luc3RhbmNlX18TRGlzcG9z
ZV9fSW5zdGFuY2VfXwAAABABAAtNeS5Db21wdXRlcgAAEwEADk15LkFwcGxpY2F0aW9uAAAMAQAH
TXkuVXNlcgAAEwEADk15LldlYlNlcnZpY2VzAAAQAQALTXkuU2V0dGluZ3MAAAAAALQAAADOyu++
AQAAAJEAAABsU3lzdGVtLlJlc291cmNlcy5SZXNvdXJjZVJlYWRlciwgbXNjb3JsaWIsIFZlcnNp
b249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRl
MDg5I1N5c3RlbS5SZXNvdXJjZXMuUnVudGltZVJlc291cmNlU2V0AgAAAAAAAAAAAAAAUEFEUEFE
ULQAAADgoAAAAAAAAAAAAAD6oAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA7KAAAAAAAAAAAAAA
AABfQ29yRGxsTWFpbgBtc2NvcmVlLmRsbAAAAAAA/yUAIAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAABABAAAAAYAACAAAAAAAAAAAAAAAAAAAABAAEAAAAwAACAAAAAAAAAAAAAAAAAAAABAAAA
AABIAAAAWMAAAFAEAAAAAAAAAAAAAFAENAAAAFYAUwBfAFYARQBSAFMASQBPAE4AXwBJAE4ARgBP
AAAAAAC9BO/+AAABAAAAAQAAAAAAAAABAAAAAAA/AAAAAAAAAAQAAAACAAAAAAAAAAAAAAAAAAAA
RAAAAAEAVgBhAHIARgBpAGwAZQBJAG4AZgBvAAAAAAAkAAQAAABUAHIAYQBuAHMAbABhAHQAaQBv
AG4AAAAAAAAAsASwAwAAAQBTAHQAcgBpAG4AZwBGAGkAbABlAEkAbgBmAG8AAACMAwAAAQAwADAA
MAAwADAANABiADAAAAAaAAEAAQBDAG8AbQBtAGUAbgB0AHMAAAAAAAAANAAKAAEAQwBvAG0AcABh
AG4AeQBOAGEAbQBlAAAAAABNAGkAYwByAG8AcwBvAGYAdAAAAH4AKwABAEYAaQBsAGUARABlAHMA
YwByAGkAcAB0AGkAbwBuAAAAAABTAFQAXwBmAGUAZQAxADkAMwAwADUAOAAxADcAMwA0ADYAYwA0
ADgANwA3ADgAZQBhAGIANABkADQAOQBjADkAMgA1ADcALgB2AGIAcAByAG8AagAAAAAAMAAIAAEA
RgBpAGwAZQBWAGUAcgBzAGkAbwBuAAAAAAAxAC4AMAAuADAALgAwAAAAfgAvAAEASQBuAHQAZQBy
AG4AYQBsAE4AYQBtAGUAAABTAFQAXwBmAGUAZQAxADkAMwAwADUAOAAxADcAMwA0ADYAYwA0ADgA
NwA3ADgAZQBhAGIANABkADQAOQBjADkAMgA1ADcALgB2AGIAcAByAG8AagAuAGQAbABsAAAAAABa
ABsAAQBMAGUAZwBhAGwAQwBvAHAAeQByAGkAZwBoAHQAAABDAG8AcAB5AHIAaQBnAGgAdAAgAEAA
IABNAGkAYwByAG8AcwBvAGYAdAAgADIAMAAxADAAAAAAACoAAQABAEwAZQBnAGEAbABUAHIAYQBk
AGUAbQBhAHIAawBzAAAAAAAAAAAAhgAvAAEATwByAGkAZwBpAG4AYQBsAEYAaQBsAGUAbgBhAG0A
ZQAAAFMAVABfAGYAZQBlADEAOQAzADAANQA4ADEANwAzADQANgBjADQAOAA3ADcAOABlAGEAYgA0
AGQANAA5AGMAOQAyADUANwAuAHYAYgBwAHIAbwBqAC4AZABsAGwAAAAAAHYAKwABAFAAcgBvAGQA
dQBjAHQATgBhAG0AZQAAAAAAUwBUAF8AZgBlAGUAMQA5ADMAMAA1ADgAMQA3ADMANAA2AGMANAA4
ADcANwA4AGUAYQBiADQAZAA0ADkAYwA5ADIANQA3AC4AdgBiAHAAcgBvAGoAAAAAADQACAABAFAA
cgBvAGQAdQBjAHQAVgBlAHIAcwBpAG8AbgAAADEALgAwAC4AMAAuADAAAAA4AAgAAQBBAHMAcwBl
AG0AYgBsAHkAIABWAGUAcgBzAGkAbwBuAAAAMQAuADAALgAwAC4AMAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKAAAAwAAAAMMQAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=</BinaryItem>
        </ScriptProject>
      </DTS:ObjectData>
    </DTS:Executable>
    <DTS:Executable
      DTS:refId="Package\Transfer Pictures"
      DTS:CreationName="Microsoft.ScriptTask"
      DTS:Description="Transfer Pictures"
      DTS:Disabled="True"
      DTS:DTSID="{D18D7934-A313-4347-BF80-6953C21CDEFD}"
      DTS:ExecutableType="Microsoft.ScriptTask"
      DTS:LocaleID="-1"
      DTS:ObjectName="Transfer Pictures"
      DTS:ThreadHint="1">
      <DTS:Variables />
      <DTS:ObjectData>
        <ScriptProject
          Name="ST_879e0c826064474aa717d03e04d56c29"
          VSTAMajorVersion="15"
          VSTAMinorVersion="0"
          Language="VisualBasic">
          <ProjectItem
            Name="\my project\settings.settings"
            Encoding="UTF8"><![CDATA[<?xml version='1.0' encoding='iso-8859-1'?>
<SettingsFile xmlns="uri:settings" CurrentProfile="(Default)" GeneratedClassNamespace="$safeprojectname" GeneratedClassName="MySettings">
  <Profiles>
    <Profile Name="(Default)" />
  </Profiles>
  <Settings />
</SettingsFile>]]></ProjectItem>
          <ProjectItem
            Name="\my project\assemblyinfo.vb"
            Encoding="UTF8"><![CDATA[Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("ST_879e0c826064474aa717d03e04d56c29.vbproj")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("ST_879e0c826064474aa717d03e04d56c29.vbproj")> 
<Assembly: AssemblyCopyright("Copyright @ Microsoft 2010")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 

<Assembly: ComVisible(False)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("68038064-b2cf-443c-ba03-99351aeb4e63")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> ]]></ProjectItem>
          <ProjectItem
            Name="Project"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="UTF-16" standalone="yes"?>
<c:Project xmlns:c="http://schemas.microsoft.com/codeprojectml/2010/08/main" xmlns:msb="http://schemas.microsoft.com/developer/msbuild/2003" runtimeVersion="4.0" schemaVersion="1.0">
	<msb:PropertyGroup>
		<msb:CodeName>st_879e0c826064474aa717d03e04d56c29</msb:CodeName>
		<msb:Language>msBuild</msb:Language>
		<msb:DisplayName>st_879e0c826064474aa717d03e04d56c29</msb:DisplayName>
		<msb:ProjectId>{3DE2E624-ABC8-4028-93C3-3A103E202A37}</msb:ProjectId>
	</msb:PropertyGroup>
	<msb:ItemGroup>
		<msb:Project Include="st_879e0c826064474aa717d03e04d56c29.vbproj"/>
		<msb:File Include="My Project\AssemblyInfo.vb"/>
		<msb:File Include="ScriptMain.vb"/>
		<msb:File Include="My Project\Resources.resx"/>
		<msb:File Include="My Project\Resources.Designer.vb"/>
		<msb:File Include="My Project\Settings.settings"/>
		<msb:File Include="My Project\Settings.Designer.vb"/>
	</msb:ItemGroup>
</c:Project>]]></ProjectItem>
          <ProjectItem
            Name="My Project\Settings.Designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



Partial Friend NotInheritable Class MySettings
    Inherits System.Configuration.ApplicationSettingsBase

    Private Shared m_Value As MySettings

    Private Shared m_SyncObject As Object = New Object

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Shared ReadOnly Property Value() As MySettings
        Get
            If (MySettings.m_Value Is Nothing) Then
                System.Threading.Monitor.Enter(MySettings.m_SyncObject)
                If (MySettings.m_Value Is Nothing) Then
                    Try
                        MySettings.m_Value = New MySettings
                    Finally
                        System.Threading.Monitor.Exit(MySettings.m_SyncObject)
                    End Try
                End If
            End If
            Return MySettings.m_Value
        End Get
    End Property
End Class]]></ProjectItem>
          <ProjectItem
            Name="My Project\Resources.Designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On


Namespace My.Resources
    
    '''<summary>
    '''   A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    'This class was auto-generated by the Strongly Typed Resource Builder
    'class via a tool like ResGen or Visual Studio.NET.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    Class MyResources
        
        Private Shared _resMgr As System.Resources.ResourceManager
        
        Private Shared _resCulture As System.Globalization.CultureInfo
        
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''   Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared ReadOnly Property ResourceManager() As System.Resources.ResourceManager
            Get
                If (_resMgr Is Nothing) Then
                    Dim temp As System.Resources.ResourceManager = New System.Resources.ResourceManager("My.Resources.MyResources", GetType(MyResources).Assembly)
                    _resMgr = temp
                End If
                Return _resMgr
            End Get
        End Property
        
        '''<summary>
        '''   Overrides the current thread's CurrentUICulture property for all
        '''   resource lookups using this strongly typed resource class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared Property Culture() As System.Globalization.CultureInfo
            Get
                Return _resCulture
            End Get
            Set
                _resCulture = value
            End Set
        End Property
    End Class
End Namespace]]></ProjectItem>
          <ProjectItem
            Name="\my project\resources.resx"
            Encoding="UTF8"><![CDATA[<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>]]></ProjectItem>
          <ProjectItem
            Name="ScriptMain.vb"
            Encoding="UTF8"><![CDATA[' Microsoft SQL Server Integration Services Script Task
' Write scripts using Microsoft Visual Basic 2008.
' The ScriptMain is the entry point class of the script.

Imports System
Imports System.Data
Imports System.Math
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.SqlClient

<Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute> _
<System.CLSCompliantAttribute(False)> _
Partial Public Class ScriptMain
    Inherits Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase

    Enum ScriptResults
        Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success
        Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
    End Enum


    ' The execution engine calls this method when the task executes.
    ' To access the object model, use the Dts property. Connections, variables, events,
    ' and logging features are available as members of the Dts property as shown in the following examples.
    '
    ' To reference a variable, call Dts.Variables("MyCaseSensitiveVariableName").Value
    ' To post a log entry, call Dts.Log("This is my log text", 999, Nothing)
    ' To fire an event, call Dts.Events.FireInformation(99, "test", "hit the help message", "", 0, True)
    '
    ' To use the connections collection use something like the following:
    ' ConnectionManager cm = Dts.Connections.Add("OLEDB")
    ' cm.ConnectionString = "Data Source=localhost;Initial Catalog=AdventureWorks;Provider=SQLNCLI10;Integrated Security=SSPI;Auto Translate=False;"
    '
    ' Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
    ' 
    ' To open Help, press F1.

    Public Sub Main()
        '
        Dim cm As ConnectionManager = Dts.Connections("LaxBadgeSql.LAXID")
        Dim cnFrom As SqlConnection = cm.AcquireConnection(Nothing) 'New SqlConnection("Server=LaxBadgeSql;Trusted_Connection=Yes;Database=LAXID")
        Dim cmdFrom As New SqlCommand("SELECT Picture FROM Badge b inner join PERSON p on b.emp_id=p.emp_id WHERE BadgeNo=@Num OR CardNo=@Num", cnFrom)

        Dim cm2 As ConnectionManager = Dts.Connections("pp4test")
        Dim cnTo As Odbc.OdbcConnection = cm2.AcquireConnection(Nothing) ' New Odbc.OdbcConnection("dsn=pp4test")
        Dim refCmd As New Odbc.OdbcCommand("select bid, person_id from badge where person_id not in (select person_id from images)", cnTo)
        Dim insertCmd As New Odbc.OdbcCommand("INSERT INTO images (person_id, type, size, width, height, version, compression, image_data, creation_date, creation_time, modify_date, modify_time) " & _
                                                " VALUES (?, 0, ?, 140, 160, '2.4.2',1, ?, ?,?,?,?)", cnTo)
        'cnFrom.Open()
        'cnTo.Open()
        Dim reader As Odbc.OdbcDataReader = refCmd.ExecuteReader()
        While reader.Read()
            cmdFrom.Parameters.Clear()
            cmdFrom.Parameters.AddWithValue("Num", reader("bid").ToString().Substring(5).Trim())
            Dim FrReader As SqlDataReader = cmdFrom.ExecuteReader()
            If Not FrReader.Read() Then
                FrReader.Close()
                Continue While
            End If
            If FrReader.IsDBNull(FrReader.GetOrdinal("Picture")) Then
                FrReader.Close()
                Continue While ' no image in badge database
            End If
            Dim image() As Byte = FrReader("Picture")
            FrReader.Close()

            image = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, System.Text.Encoding.Default, image)
            insertCmd.Parameters.Clear()
            insertCmd.Parameters.AddWithValue("person_id", reader("person_id"))
            insertCmd.Parameters.AddWithValue("size", image.Length)
            insertCmd.Parameters.AddWithValue("image_data", image)
            insertCmd.Parameters.AddWithValue("creation_date", Now.ToString("yyyyMMdd"))
            insertCmd.Parameters.AddWithValue("creation_time", Now.ToString("HHmmss"))
            insertCmd.Parameters.AddWithValue("modify_date", Now.ToString("yyyyMMdd"))
            insertCmd.Parameters.AddWithValue("modify_time", Now.ToString("HHmmss"))
            Try
                insertCmd.ExecuteNonQuery()
            Catch ex As Exception
                Dts.Log("Exception in insert: " + ex.Message, 999, Nothing)
            End Try
        End While
        '
        Dts.TaskResult = ScriptResults.Success

    End Sub

End Class]]></ProjectItem>
          <ProjectItem
            Name="\st_879e0c826064474aa717d03e04d56c29.vbproj"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="utf-16"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <!-- This section defines project-level properties.

       Configuration - Specifies whether the default configuration is Release or Debug.
       Platform - Specifies what CPU the output of this project can run on.
       OutputType - Must be "Library" for VSTA.
       NoStandardLibraries - Set to "false" for VSTA.
       RootNamespace - In C#, this specifies the namespace given to new files.
                       In Visual Basic, all objects are wrapped in this namespace at runtime.
       AssemblyName - Name of the output assembly.
  -->
  <PropertyGroup>
    <ProjectTypeGuids>{30D016F9-3734-4E33-A861-5E7D899E18F3};{F184B08F-C81C-45F6-A57F-5ABD9991F28F}</ProjectTypeGuids>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <RootNamespace>ST_879e0c826064474aa717d03e04d56c29.vbproj</RootNamespace>
    <AssemblyName>ST_879e0c826064474aa717d03e04d56c29.vbproj</AssemblyName>
    <StartupObject></StartupObject>
    <OptionExplicit>On</OptionExplicit>
    <OptionCompare>Binary</OptionCompare>
    <OptionStrict>Off</OptionStrict>
    <OptionInfer>On</OptionInfer>
    <ProjectGuid>{6809055F-B5FC-4208-8C3B-962BF30B4399}</ProjectGuid>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <TargetFrameworkProfile></TargetFrameworkProfile>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Debug" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>true</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>true</DebugSymbols>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Release" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>false</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>false</DebugSymbols>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section enables pre- and post-build steps. However,
       it is recommended that MSBuild tasks be used instead of these properties.
  -->
  <PropertyGroup>
    <PreBuildEvent></PreBuildEvent>
    <PostBuildEvent></PostBuildEvent>
  </PropertyGroup>
  <!-- This sections specifies references for the project. -->
  <ItemGroup>
    <Reference Include="adodb, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
    <Reference Include="Microsoft.SqlServer.ManagedDTS, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
    <Reference Include="Microsoft.SqlServer.ScriptTask, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
  </ItemGroup>
  <!-- Visual Basic supports Importing namespaces (equivalent to using statements in C#).-->
  <ItemGroup>
    <Import Include="Microsoft.VisualBasic" />
    <Import Include="System" />
    <Import Include="System.Collections" />
    <Import Include="System.Data" />
    <Import Include="System.Diagnostics" />
    <Import Include="System.Windows.Forms" />
  </ItemGroup>
  <!-- This section defines the user source files that are part of the
       project.

       Compile - Specifies a source file to compile.
       EmbeddedResource - Specifies a .resx file for embedded resources.
       None - Specifies a file that is not to be passed to the compiler (for instance,
              a text file or XML file).
       AppDesigner - Specifies the directory where the application properties files can
                     be found.
  -->
  <ItemGroup>
    <AppDesigner Include="My Project\" />
    <Compile Include="My Project\AssemblyInfo.vb">
      <SubType>Code</SubType>
    </Compile>
    <EmbeddedResource Include="My Project\Resources.resx">
      <Generator>VbMyResourcesResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.vb</LastGenOutput>
      <CustomToolNamespace>My.Resources</CustomToolNamespace>
    </EmbeddedResource>
    <Compile Include="My Project\Resources.Designer.vb">
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
      <DependentUpon>Resources.resx</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <None Include="My Project\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.vb</LastGenOutput>
    </None>
    <Compile Include="My Project\Settings.Designer.vb">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="ScriptMain.vb">
      <SubType>Code</SubType>
    </Compile>
    <!-- Include the default configuration information and metadata files for the add-in.
         These files are copied to the build output directory when the project is
         built, and the path to the configuration file is passed to add-in on the command
         line when debugging.
    -->
  </ItemGroup>
  <!-- Include the build rules for a VB project.-->
  <Import Project="$(MSBuildBinPath)\Microsoft.VisualBasic.targets" />
  <!-- This section defines VSTA properties that describe the host-changable project properties. -->
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID="{30D016F9-3734-4E33-A861-5E7D899E18F3}">
        <ProjectProperties HostName="VSTAHostName" HostPackage="{B3A685AA-7EAF-4BC6-9940-57959FA5AC07}" ApplicationType="usd" Language="vb" TemplatesPath="" DebugInfoExeName="" />
        <Host Name="SSIS_ScriptTask" />
        <ProjectClient>
          <HostIdentifier>SSIS_ST140</HostIdentifier>
        </ProjectClient>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
</Project>]]></ProjectItem>
          <ProjectItem
            Name="My Project\AssemblyInfo.vb"
            Encoding="UTF8"><![CDATA[Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("ST_879e0c826064474aa717d03e04d56c29.vbproj")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("ST_879e0c826064474aa717d03e04d56c29.vbproj")> 
<Assembly: AssemblyCopyright("Copyright @ Microsoft 2010")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 

<Assembly: ComVisible(False)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("68038064-b2cf-443c-ba03-99351aeb4e63")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> ]]></ProjectItem>
          <ProjectItem
            Name="st_879e0c826064474aa717d03e04d56c29.vbproj"
            Encoding="UTF16LE"><![CDATA[<?xml version="1.0" encoding="utf-16"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <!-- This section defines project-level properties.

       Configuration - Specifies whether the default configuration is Release or Debug.
       Platform - Specifies what CPU the output of this project can run on.
       OutputType - Must be "Library" for VSTA.
       NoStandardLibraries - Set to "false" for VSTA.
       RootNamespace - In C#, this specifies the namespace given to new files.
                       In Visual Basic, all objects are wrapped in this namespace at runtime.
       AssemblyName - Name of the output assembly.
  -->
  <PropertyGroup>
    <ProjectTypeGuids>{30D016F9-3734-4E33-A861-5E7D899E18F3};{F184B08F-C81C-45F6-A57F-5ABD9991F28F}</ProjectTypeGuids>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <RootNamespace>ST_879e0c826064474aa717d03e04d56c29.vbproj</RootNamespace>
    <AssemblyName>ST_879e0c826064474aa717d03e04d56c29.vbproj</AssemblyName>
    <StartupObject></StartupObject>
    <OptionExplicit>On</OptionExplicit>
    <OptionCompare>Binary</OptionCompare>
    <OptionStrict>Off</OptionStrict>
    <OptionInfer>On</OptionInfer>
    <ProjectGuid>{6809055F-B5FC-4208-8C3B-962BF30B4399}</ProjectGuid>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <TargetFrameworkProfile></TargetFrameworkProfile>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Debug" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>true</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>true</DebugSymbols>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section defines properties that are set when the "Release" configuration is
       selected.

       DebugSymbols - If true, create symbols (.pdb). If false, do not create symbols.
       Optimize - If true, optimize the build output. If false, do not optimize.
       OutputPath - Output path of the project relative to the project file.
       EnableUnmanagedDebugging - If true, starting the debugger will attach both managed and unmanaged debuggers.
       DefineConstants - Constants defined for the preprocessor.
       Warning Level - Warning level for the compiler.
  -->
  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <DefineConstants></DefineConstants>
    <DefineDebug>false</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <DebugSymbols>false</DebugSymbols>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <RegisterForComInterop>false</RegisterForComInterop>
    <RemoveIntegerChecks>false</RemoveIntegerChecks>
    <NoWarn>42016,42017,42018,42019,42032,42353,42354,42355,42353,42354,42355</NoWarn>
  </PropertyGroup>
  <!-- This section enables pre- and post-build steps. However,
       it is recommended that MSBuild tasks be used instead of these properties.
  -->
  <PropertyGroup>
    <PreBuildEvent></PreBuildEvent>
    <PostBuildEvent></PostBuildEvent>
  </PropertyGroup>
  <!-- This sections specifies references for the project. -->
  <ItemGroup>
    <Reference Include="adodb, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
    <Reference Include="Microsoft.SqlServer.ManagedDTS, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
    <Reference Include="Microsoft.SqlServer.ScriptTask, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=89845dcd8080cc91" />
  </ItemGroup>
  <!-- Visual Basic supports Importing namespaces (equivalent to using statements in C#).-->
  <ItemGroup>
    <Import Include="Microsoft.VisualBasic" />
    <Import Include="System" />
    <Import Include="System.Collections" />
    <Import Include="System.Data" />
    <Import Include="System.Diagnostics" />
    <Import Include="System.Windows.Forms" />
  </ItemGroup>
  <!-- This section defines the user source files that are part of the
       project.

       Compile - Specifies a source file to compile.
       EmbeddedResource - Specifies a .resx file for embedded resources.
       None - Specifies a file that is not to be passed to the compiler (for instance,
              a text file or XML file).
       AppDesigner - Specifies the directory where the application properties files can
                     be found.
  -->
  <ItemGroup>
    <AppDesigner Include="My Project\" />
    <Compile Include="My Project\AssemblyInfo.vb">
      <SubType>Code</SubType>
    </Compile>
    <EmbeddedResource Include="My Project\Resources.resx">
      <Generator>VbMyResourcesResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.vb</LastGenOutput>
      <CustomToolNamespace>My.Resources</CustomToolNamespace>
    </EmbeddedResource>
    <Compile Include="My Project\Resources.Designer.vb">
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
      <DependentUpon>Resources.resx</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <None Include="My Project\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.vb</LastGenOutput>
    </None>
    <Compile Include="My Project\Settings.Designer.vb">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="ScriptMain.vb">
      <SubType>Code</SubType>
    </Compile>
    <!-- Include the default configuration information and metadata files for the add-in.
         These files are copied to the build output directory when the project is
         built, and the path to the configuration file is passed to add-in on the command
         line when debugging.
    -->
  </ItemGroup>
  <!-- Include the build rules for a VB project.-->
  <Import Project="$(MSBuildBinPath)\Microsoft.VisualBasic.targets" />
  <!-- This section defines VSTA properties that describe the host-changable project properties. -->
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID="{30D016F9-3734-4E33-A861-5E7D899E18F3}">
        <ProjectProperties HostName="VSTAHostName" HostPackage="{B3A685AA-7EAF-4BC6-9940-57959FA5AC07}" ApplicationType="usd" Language="vb" TemplatesPath="" DebugInfoExeName="" />
        <Host Name="SSIS_ScriptTask" />
        <ProjectClient>
          <HostIdentifier>SSIS_ST140</HostIdentifier>
        </ProjectClient>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
</Project>]]></ProjectItem>
          <ProjectItem
            Name="\my project\settings.designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



Partial Friend NotInheritable Class MySettings
    Inherits System.Configuration.ApplicationSettingsBase

    Private Shared m_Value As MySettings

    Private Shared m_SyncObject As Object = New Object

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Shared ReadOnly Property Value() As MySettings
        Get
            If (MySettings.m_Value Is Nothing) Then
                System.Threading.Monitor.Enter(MySettings.m_SyncObject)
                If (MySettings.m_Value Is Nothing) Then
                    Try
                        MySettings.m_Value = New MySettings
                    Finally
                        System.Threading.Monitor.Exit(MySettings.m_SyncObject)
                    End Try
                End If
            End If
            Return MySettings.m_Value
        End Get
    End Property
End Class]]></ProjectItem>
          <ProjectItem
            Name="My Project\Settings.settings"
            Encoding="UTF8"><![CDATA[<?xml version='1.0' encoding='iso-8859-1'?>
<SettingsFile xmlns="uri:settings" CurrentProfile="(Default)" GeneratedClassNamespace="$safeprojectname" GeneratedClassName="MySettings">
  <Profiles>
    <Profile Name="(Default)" />
  </Profiles>
  <Settings />
</SettingsFile>]]></ProjectItem>
          <ProjectItem
            Name="\scriptmain.vb"
            Encoding="UTF8"><![CDATA[' Microsoft SQL Server Integration Services Script Task
' Write scripts using Microsoft Visual Basic 2008.
' The ScriptMain is the entry point class of the script.

Imports System
Imports System.Data
Imports System.Math
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.SqlClient

<Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute> _
<System.CLSCompliantAttribute(False)> _
Partial Public Class ScriptMain
    Inherits Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase

    Enum ScriptResults
        Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success
        Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
    End Enum


    ' The execution engine calls this method when the task executes.
    ' To access the object model, use the Dts property. Connections, variables, events,
    ' and logging features are available as members of the Dts property as shown in the following examples.
    '
    ' To reference a variable, call Dts.Variables("MyCaseSensitiveVariableName").Value
    ' To post a log entry, call Dts.Log("This is my log text", 999, Nothing)
    ' To fire an event, call Dts.Events.FireInformation(99, "test", "hit the help message", "", 0, True)
    '
    ' To use the connections collection use something like the following:
    ' ConnectionManager cm = Dts.Connections.Add("OLEDB")
    ' cm.ConnectionString = "Data Source=localhost;Initial Catalog=AdventureWorks;Provider=SQLNCLI10;Integrated Security=SSPI;Auto Translate=False;"
    '
    ' Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
    ' 
    ' To open Help, press F1.

    Public Sub Main()
        '
        Dim cm As ConnectionManager = Dts.Connections("LaxBadgeSql.LAXID")
        Dim cnFrom As SqlConnection = cm.AcquireConnection(Nothing) 'New SqlConnection("Server=LaxBadgeSql;Trusted_Connection=Yes;Database=LAXID")
        Dim cmdFrom As New SqlCommand("SELECT Picture FROM Badge b inner join PERSON p on b.emp_id=p.emp_id WHERE BadgeNo=@Num OR CardNo=@Num", cnFrom)

        Dim cm2 As ConnectionManager = Dts.Connections("pp4test")
        Dim cnTo As Odbc.OdbcConnection = cm2.AcquireConnection(Nothing) ' New Odbc.OdbcConnection("dsn=pp4test")
        Dim refCmd As New Odbc.OdbcCommand("select bid, person_id from badge where person_id not in (select person_id from images)", cnTo)
        Dim insertCmd As New Odbc.OdbcCommand("INSERT INTO images (person_id, type, size, width, height, version, compression, image_data, creation_date, creation_time, modify_date, modify_time) " & _
                                                " VALUES (?, 0, ?, 140, 160, '2.4.2',1, ?, ?,?,?,?)", cnTo)
        'cnFrom.Open()
        'cnTo.Open()
        Dim reader As Odbc.OdbcDataReader = refCmd.ExecuteReader()
        While reader.Read()
            cmdFrom.Parameters.Clear()
            cmdFrom.Parameters.AddWithValue("Num", reader("bid").ToString().Substring(5).Trim())
            Dim FrReader As SqlDataReader = cmdFrom.ExecuteReader()
            If Not FrReader.Read() Then
                FrReader.Close()
                Continue While
            End If
            If FrReader.IsDBNull(FrReader.GetOrdinal("Picture")) Then
                FrReader.Close()
                Continue While ' no image in badge database
            End If
            Dim image() As Byte = FrReader("Picture")
            FrReader.Close()

            image = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, System.Text.Encoding.Default, image)
            insertCmd.Parameters.Clear()
            insertCmd.Parameters.AddWithValue("person_id", reader("person_id"))
            insertCmd.Parameters.AddWithValue("size", image.Length)
            insertCmd.Parameters.AddWithValue("image_data", image)
            insertCmd.Parameters.AddWithValue("creation_date", Now.ToString("yyyyMMdd"))
            insertCmd.Parameters.AddWithValue("creation_time", Now.ToString("HHmmss"))
            insertCmd.Parameters.AddWithValue("modify_date", Now.ToString("yyyyMMdd"))
            insertCmd.Parameters.AddWithValue("modify_time", Now.ToString("HHmmss"))
            Try
                insertCmd.ExecuteNonQuery()
            Catch ex As Exception
                Dts.Log("Exception in insert: " + ex.Message, 999, Nothing)
            End Try
        End While
        '
        Dts.TaskResult = ScriptResults.Success

    End Sub

End Class]]></ProjectItem>
          <ProjectItem
            Name="My Project\Resources.resx"
            Encoding="UTF8"><![CDATA[<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>]]></ProjectItem>
          <ProjectItem
            Name="\my project\resources.designer.vb"
            Encoding="UTF8"><![CDATA['------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On


Namespace My.Resources
    
    '''<summary>
    '''   A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    'This class was auto-generated by the Strongly Typed Resource Builder
    'class via a tool like ResGen or Visual Studio.NET.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    Class MyResources
        
        Private Shared _resMgr As System.Resources.ResourceManager
        
        Private Shared _resCulture As System.Globalization.CultureInfo
        
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''   Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared ReadOnly Property ResourceManager() As System.Resources.ResourceManager
            Get
                If (_resMgr Is Nothing) Then
                    Dim temp As System.Resources.ResourceManager = New System.Resources.ResourceManager("My.Resources.MyResources", GetType(MyResources).Assembly)
                    _resMgr = temp
                End If
                Return _resMgr
            End Get
        End Property
        
        '''<summary>
        '''   Overrides the current thread's CurrentUICulture property for all
        '''   resource lookups using this strongly typed resource class.
        '''</summary>
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Public Shared Property Culture() As System.Globalization.CultureInfo
            Get
                Return _resCulture
            End Get
            Set
                _resCulture = value
            End Set
        End Property
    End Class
End Namespace]]></ProjectItem>
          <BinaryItem
            Name="ST_879e0c826064474aa717d03e04d56c29.vbproj.dll">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
AAAAAIMAkwH7AQMADiQAAAAAxgLVBMUAAwAYJAAAAAARAG8AAAIDADgkAAAAAAEAWwAIAgQAuiAA
AAAABhhOCAYABQBEJAAAAAADCP0ASgAFAHgkAAAAAAYYTggGAAUAAAABAJsEAAABAAcHAAABAA0B
AAABAA0BCQBOCAEAEQBOCAYAGQBOCAoAKQBOCBAAMQBOCBAAOQBOCBAAQQBOCBAASQBOCBAAUQBO
CBAAWQBOCBUAYQBOCBUAaQBOCBAAcQBOCBAAeQBOCBAAgQBOCBoAkQBOCCAAqQBOCAYAsQBOCAYA
uQBOCAYA0QBOCCYA6QBOCBAACQFOCAYAEQFOCAYAmQBOCAYAoQBOCAYADABOCAYAFABOCAYAHABO
CAYAJABOCAYADAD9AEoAFAD9AEoAHAD9AEoAJAD9AEoAwQBOCAYA2QBMAU8A2QDrClcA8QBOCF0A
gQGSBGUAAQFOCAYAiQH8B2oAiQFhCmoAGQFOCAYAGQEtCowAkQH3CZIAmQH5BZgAoQHTBp8AMQFO
CKQAQQFOCKwAQQEsB7QAMQEWCroAqQEJBwYASQH5BcAAwQDVBMUAsQHeBMkAsQEJBsUAqQGFBM4A
MQEsB9YAUQHSANwAUQELAgYAUQFkBeAAUQHwBeUAUQH5BcAAwQEiAe0AwQFmCu0AwQGXCvMAQQEW
CgAByQEJBwYAyQGFBAYB4QG1Cg4BWQHVBBQBQQH4ChkB6QE0CB0BYQEuAcUAsQFDCiQBkQHoBCoB
6QEiCDIBSQHSANwAkQFyCgEAwQDwCTYBwQAWARkB8QHuAEMBPACrCloBRABxBEoARAB7BHEBRABO
CAYACAAsAJIBCAAwAJcBKQCrANkDLgALADgCLgATAEECLgAbAGACLgAjAGkCLgArAJkCLgAzAJ8C
LgA7AGkCLgBDAK4CLgBLAJkCLgBTAM4CLgBbAJkCLgBjANQCLgBrAP4CLgBzAAsDQACLAJcBQACD
AFUDQwB7AF4DQwCDAFUDSQCrAOoDYwB7AF4DYwCDAFUDaQCrAP4DgACLAJcBgwCTAJcBgwCbAJcB
gwB7AF4DiQCrAAsEoACLAJcBqQCDAGACwACLAJcByQCDAGAC4ACLAJcB4wC7AJcB4wBTAJkC6QCz
AJcBAwGDAFUDAwGjAHcDIwGDAFUDIwFbAJkCIAKDAFUDIAKLAJcBQAKDAFUDQAKLAJcBYAKDAFUD
YAKLAJcBgAKDAFUDgAKLAJcBoAKLAJcBwAKLAJcB4AKLAJcB4AKDAFUDAAOLAJcBIAOLAJcBIAOD
AFUDbwA7AU4BBAABAAUABQAGAAcACQAIAAAAEQgQAgAATQYVAgAA3AcaAgAApggfAgAAngckAgAA
vwEpAgAAmwQuAgAAAQEzAgIABAADAAIABQAFAAIABgAHAAIABwAJAAIACQALAAIACgANAAEACwAN
AAIADgAPAAIAGAARAC4ANQA8AEMA6gBAAVMBYgFpAQSAAAABAAAAAAAAAAAAAAAAAPEEAAAEAAAA
AAAAAAAAAAB3AaIAAAAAAAQAAAAAAAAAAAAAAHcBAgYAAAAACgAAAAAAAAAAAAAAgAG8AAAAAAAO
AAAAAAAAAAAAAACJARwFAAAAAAQAAAAAAAAAAAAAAHcBigAAAAAADgAAAAAAAAAAAAAAiQE6AAAA
AAAAAAAAAQAAAJcJAAAIAAQACQAEAAoABwAAABAAEgBZAAAAEAArAFkAAAAAAC0AWQCjAEkBowBs
AQAAAAAAQ29udGV4dFZhbHVlYDEAVGhyZWFkU2FmZU9iamVjdFByb3ZpZGVyYDEASW50MzIAPE1v
ZHVsZT4ATWljcm9zb2Z0LlNxbFNlcnZlci5NYW5hZ2VkRFRTAFQARGlzcG9zZV9fSW5zdGFuY2Vf
XwBDcmVhdGVfX0luc3RhbmNlX18AdmFsdWVfXwBTeXN0ZW0uRGF0YQBQcm9qZWN0RGF0YQBtc2Nv
cmxpYgBTeXN0ZW0uRGF0YS5PZGJjAE1pY3Jvc29mdC5WaXN1YWxCYXNpYwBSZWFkAE9kYmNDb21t
YW5kAFNxbENvbW1hbmQAQ3JlYXRlSW5zdGFuY2UAZ2V0X0dldEluc3RhbmNlAGluc3RhbmNlAEdl
dEhhc2hDb2RlAGdldF9Vbmljb2RlAGdldF9NZXNzYWdlAFJ1bnRpbWVUeXBlSGFuZGxlAEdldFR5
cGVGcm9tSGFuZGxlAERhdGVBbmRUaW1lAERhdGVUaW1lAE1pY3Jvc29mdC5TcWxTZXJ2ZXIuRHRz
LlJ1bnRpbWUAR2V0VHlwZQBGYWlsdXJlAGdldF9DdWx0dXJlAHNldF9DdWx0dXJlAF9yZXNDdWx0
dXJlAFZTVEFSVFNjcmlwdE9iamVjdE1vZGVsQmFzZQBBcHBsaWNhdGlvbkJhc2UAQXBwbGljYXRp
b25TZXR0aW5nc0Jhc2UAQ2xvc2UARWRpdG9yQnJvd3NhYmxlU3RhdGUAR3VpZEF0dHJpYnV0ZQBI
ZWxwS2V5d29yZEF0dHJpYnV0ZQBHZW5lcmF0ZWRDb2RlQXR0cmlidXRlAERlYnVnZ2VyTm9uVXNl
ckNvZGVBdHRyaWJ1dGUARGVidWdnYWJsZUF0dHJpYnV0ZQBFZGl0b3JCcm93c2FibGVBdHRyaWJ1
dGUAQ29tVmlzaWJsZUF0dHJpYnV0ZQBBc3NlbWJseVRpdGxlQXR0cmlidXRlAFN0YW5kYXJkTW9k
dWxlQXR0cmlidXRlAEhpZGVNb2R1bGVOYW1lQXR0cmlidXRlAEFzc2VtYmx5VHJhZGVtYXJrQXR0
cmlidXRlAFRhcmdldEZyYW1ld29ya0F0dHJpYnV0ZQBEZWJ1Z2dlckhpZGRlbkF0dHJpYnV0ZQBB
c3NlbWJseUZpbGVWZXJzaW9uQXR0cmlidXRlAE15R3JvdXBDb2xsZWN0aW9uQXR0cmlidXRlAEFz
c2VtYmx5RGVzY3JpcHRpb25BdHRyaWJ1dGUAQ29tcGlsYXRpb25SZWxheGF0aW9uc0F0dHJpYnV0
ZQBBc3NlbWJseVByb2R1Y3RBdHRyaWJ1dGUAQXNzZW1ibHlDb3B5cmlnaHRBdHRyaWJ1dGUAQ0xT
Q29tcGxpYW50QXR0cmlidXRlAFNTSVNTY3JpcHRUYXNrRW50cnlQb2ludEF0dHJpYnV0ZQBBc3Nl
bWJseUNvbXBhbnlBdHRyaWJ1dGUAUnVudGltZUNvbXBhdGliaWxpdHlBdHRyaWJ1dGUAbV9WYWx1
ZQBnZXRfVmFsdWUAc2V0X1ZhbHVlAEFkZFdpdGhWYWx1ZQBHZXRPYmplY3RWYWx1ZQBTeXN0ZW0u
VGhyZWFkaW5nAEVuY29kaW5nAFN5c3RlbS5SdW50aW1lLlZlcnNpb25pbmcAVG9TdHJpbmcAU3Vi
c3RyaW5nAExvZwBNYXRoAFNUXzg3OWUwYzgyNjA2NDQ3NGFhNzE3ZDAzZTA0ZDU2YzI5LnZicHJv
agBNaWNyb3NvZnQuU3FsU2VydmVyLlNjcmlwdFRhc2sATWljcm9zb2Z0LlNxbFNlcnZlci5EdHMu
VGFza3MuU2NyaXB0VGFzawBHZXRPcmRpbmFsAE1pY3Jvc29mdC5WaXN1YWxCYXNpYy5NeVNlcnZp
Y2VzLkludGVybmFsAFNjcmlwdE9iamVjdE1vZGVsAFN5c3RlbS5Db21wb25lbnRNb2RlbABTVF84
NzllMGM4MjYwNjQ0NzRhYTcxN2QwM2UwNGQ1NmMyOS52YnByb2ouZGxsAElzREJOdWxsAGdldF9J
dGVtAFN5c3RlbQBUcmltAEVudW0AU3lzdGVtLkNvbXBvbmVudE1vZGVsLkRlc2lnbgBTY3JpcHRN
YWluAGdldF9BcHBsaWNhdGlvbgBNeUFwcGxpY2F0aW9uAFN5c3RlbS5Db25maWd1cmF0aW9uAFN5
c3RlbS5HbG9iYWxpemF0aW9uAFN5c3RlbS5SZWZsZWN0aW9uAE9kYmNQYXJhbWV0ZXJDb2xsZWN0
aW9uAFNxbFBhcmFtZXRlckNvbGxlY3Rpb24AT2RiY0Nvbm5lY3Rpb24AQWNxdWlyZUNvbm5lY3Rp
b24AU3FsQ29ubmVjdGlvbgBFeGNlcHRpb24AQ3VsdHVyZUluZm8AQ2xlYXIAT2RiY0RhdGFSZWFk
ZXIAU3FsRGF0YVJlYWRlcgBFeGVjdXRlUmVhZGVyAG1fQXBwT2JqZWN0UHJvdmlkZXIAbV9Vc2Vy
T2JqZWN0UHJvdmlkZXIAbV9Db21wdXRlck9iamVjdFByb3ZpZGVyAG1fTXlXZWJTZXJ2aWNlc09i
amVjdFByb3ZpZGVyAGdldF9SZXNvdXJjZU1hbmFnZXIAQ29ubmVjdGlvbk1hbmFnZXIAU3lzdGVt
LkNvZGVEb20uQ29tcGlsZXIAZ2V0X1VzZXIAT2RiY1BhcmFtZXRlcgBTcWxQYXJhbWV0ZXIARW50
ZXIAZ2V0X0NvbXB1dGVyAE15Q29tcHV0ZXIAX3Jlc01ncgBDbGVhclByb2plY3RFcnJvcgBTZXRQ
cm9qZWN0RXJyb3IAQWN0aXZhdG9yAC5jdG9yAC5jY3RvcgBNb25pdG9yAFN5c3RlbS5EaWFnbm9z
dGljcwBNaWNyb3NvZnQuVmlzdWFsQmFzaWMuRGV2aWNlcwBnZXRfV2ViU2VydmljZXMATXlXZWJT
ZXJ2aWNlcwBNaWNyb3NvZnQuVmlzdWFsQmFzaWMuQXBwbGljYXRpb25TZXJ2aWNlcwBTeXN0ZW0u
UnVudGltZS5JbnRlcm9wU2VydmljZXMATWljcm9zb2Z0LlZpc3VhbEJhc2ljLkNvbXBpbGVyU2Vy
dmljZXMAU3lzdGVtLlJ1bnRpbWUuQ29tcGlsZXJTZXJ2aWNlcwBTeXN0ZW0uUmVzb3VyY2VzAFNU
Xzg3OWUwYzgyNjA2NDQ3NGFhNzE3ZDAzZTA0ZDU2YzI5LnZicHJvai5NeS5SZXNvdXJjZXMATXlS
ZXNvdXJjZXMAU1RfODc5ZTBjODI2MDY0NDc0YWE3MTdkMDNlMDRkNTZjMjkudmJwcm9qLlJlc291
cmNlcy5yZXNvdXJjZXMARGVidWdnaW5nTW9kZXMATXlTZXR0aW5ncwBFcXVhbHMAZ2V0X0Nvbm5l
Y3Rpb25zAFJ1bnRpbWVIZWxwZXJzAGdldF9QYXJhbWV0ZXJzAFN1Y2Nlc3MAZ2V0X0R0cwBTY3Jp
cHRSZXN1bHRzAENvbmNhdABtX1N5bmNPYmplY3QATXlQcm9qZWN0AEV4aXQAZ2V0X0RlZmF1bHQA
c2V0X1Rhc2tSZXN1bHQAU3lzdGVtLkRhdGEuU3FsQ2xpZW50AENvbnZlcnQAU3lzdGVtLlRleHQA
bV9Db250ZXh0AGdldF9Ob3cAU1RfODc5ZTBjODI2MDY0NDc0YWE3MTdkMDNlMDRkNTZjMjkudmJw
cm9qLk15AGdldF9Bc3NlbWJseQBFeGVjdXRlTm9uUXVlcnkAADFNAHkALgBSAGUAcwBvAHUAcgBj
AGUAcwAuAE0AeQBSAGUAcwBvAHUAcgBjAGUAcwAAI0wAYQB4AEIAYQBkAGcAZQBTAHEAbAAuAEwA
QQBYAEkARAAAgM1TAEUATABFAEMAVAAgAFAAaQBjAHQAdQByAGUAIABGAFIATwBNACAAQgBhAGQA
ZwBlACAAYgAgAGkAbgBuAGUAcgAgAGoAbwBpAG4AIABQAEUAUgBTAE8ATgAgAHAAIABvAG4AIABi
AC4AZQBtAHAAXwBpAGQAPQBwAC4AZQBtAHAAXwBpAGQAIABXAEgARQBSAEUAIABCAGEAZABnAGUA
TgBvAD0AQABOAHUAbQAgAE8AUgAgAEMAYQByAGQATgBvAD0AQABOAHUAbQAAD3AAcAA0AHQAZQBz
AHQAAICtcwBlAGwAZQBjAHQAIABiAGkAZAAsACAAcABlAHIAcwBvAG4AXwBpAGQAIABmAHIAbwBt
ACAAYgBhAGQAZwBlACAAdwBoAGUAcgBlACAAcABlAHIAcwBvAG4AXwBpAGQAIABuAG8AdAAgAGkA
bgAgACgAcwBlAGwAZQBjAHQAIABwAGUAcgBzAG8AbgBfAGkAZAAgAGYAcgBvAG0AIABpAG0AYQBn
AGUAcwApAACBjUkATgBTAEUAUgBUACAASQBOAFQATwAgAGkAbQBhAGcAZQBzACAAKABwAGUAcgBz
AG8AbgBfAGkAZAAsACAAdAB5AHAAZQAsACAAcwBpAHoAZQAsACAAdwBpAGQAdABoACwAIABoAGUA
aQBnAGgAdAAsACAAdgBlAHIAcwBpAG8AbgAsACAAYwBvAG0AcAByAGUAcwBzAGkAbwBuACwAIABp
AG0AYQBnAGUAXwBkAGEAdABhACwAIABjAHIAZQBhAHQAaQBvAG4AXwBkAGEAdABlACwAIABjAHIA
ZQBhAHQAaQBvAG4AXwB0AGkAbQBlACwAIABtAG8AZABpAGYAeQBfAGQAYQB0AGUALAAgAG0AbwBk
AGkAZgB5AF8AdABpAG0AZQApACAAIABWAEEATABVAEUAUwAgACgAPwAsACAAMAAsACAAPwAsACAA
MQA0ADAALAAgADEANgAwACwAIAAnADIALgA0AC4AMgAnACwAMQAsACAAPwAsACAAPwAsAD8ALAA/
ACwAPwApAAEHTgB1AG0AAAdiAGkAZAAAD1AAaQBjAHQAdQByAGUAABNwAGUAcgBzAG8AbgBfAGkA
ZAAACXMAaQB6AGUAABVpAG0AYQBnAGUAXwBkAGEAdABhAAAbYwByAGUAYQB0AGkAbwBuAF8AZABh
AHQAZQAAEXkAeQB5AHkATQBNAGQAZAAAG2MAcgBlAGEAdABpAG8AbgBfAHQAaQBtAGUAAA1IAEgA
bQBtAHMAcwAAF20AbwBkAGkAZgB5AF8AZABhAHQAZQAAF20AbwBkAGkAZgB5AF8AdABpAG0AZQAA
K0UAeABjAGUAcAB0AGkAbwBuACAAaQBuACAAaQBuAHMAZQByAHQAOgAgAACc3IEvrxGTQa49Zr/V
7EnkAAQgAQEIAyAAAQUgAQEREQQgAQEOBCABAQIFIAIBDg4FIAEBEUUHIAQBDg4ODgYVEiQBEgwG
FRIkARIIBhUSJAESZQYVEiQBEiAEIAATAAcAARJtEYC5BSAAEoC9ByACAQ4SgL0EAAEcHAQAAQEc
HAcJEoCVEoCZEoCdEoChEoClEoCpHQURgK0SgLEFIAASgMkFIAASgM0GIAESgNEcBCABHBwHIAIB
DhKAlQcgAgEOEoCdBSAAEoClBSAAEoDVBCABHA4DIAAOBCABDggHIAISgN0OHAUgABKAqQMgAAIE
IAEIDgQgAQIIAh0FBQAAEoDhDAADHQUSgOESgOEdBQUgABKA5QcgAhKA6Q4cBQAAEYCtBCABDg4D
IAAIBgABARKAsQUAAg4ODgcgAwEOCB0FAwAAAQQgAQIcBAcBHgACHgAFEAEAHgAECgEeAAQHARMA
BhUSJAETAAcGFRJxARMABhUScQETAAITAAQKARMABSABARMACLd6XFYZNOCJCLA/X38R1Qo6CImE
Xc2AgMyRBAAAAAAEAQAAAAcGFRIkARIMBwYVEiQBEggHBhUSJAESZQcGFRIkARIgAwYSeQMGEn0D
BhIYAgYcAgYIAwYRKAQAABIMBAAAEggEAAASZQQAABIgBAAAEnkEAAASfQUAAQESfQQAABIYBCAA
Em0HEAEBHgAeAAcwAQEBEB4ABAgAEgwECAASCAQIABJlBAgAEiAECAASeQQIABJ9BAgAEhgEKAAT
AAgBAAgAAAAAAB4BAAEAVAIWV3JhcE5vbkV4Y2VwdGlvblRocm93cwEIAQACAAAAAAAvAQAqU1Rf
ODc5ZTBjODI2MDY0NDc0YWE3MTdkMDNlMDRkNTZjMjkudmJwcm9qAAAFAQAAAAAOAQAJTWljcm9z
b2Z0AAAfAQAaQ29weXJpZ2h0IEAgTWljcm9zb2Z0IDIwMTAAAAUBAAEAACkBACQ2ODAzODA2NC1i
MmNmLTQ0M2MtYmEwMy05OTM1MWFlYjRlNjMAAAwBAAcxLjAuMC4wAABJAQAaLk5FVEZyYW1ld29y
ayxWZXJzaW9uPXY0LjUBAFQOFEZyYW1ld29ya0Rpc3BsYXlOYW1lEi5ORVQgRnJhbWV3b3JrIDQu
NQgBAAEAAAAAABgBAApNeVRlbXBsYXRlCDExLjAuMC4wAABhAQA0U3lzdGVtLldlYi5TZXJ2aWNl
cy5Qcm90b2NvbHMuU29hcEh0dHBDbGllbnRQcm90b2NvbBJDcmVhdGVfX0luc3RhbmNlX18TRGlz
cG9zZV9fSW5zdGFuY2VfXwAAABABAAtNeS5Db21wdXRlcgAAEwEADk15LkFwcGxpY2F0aW9uAAAM
AQAHTXkuVXNlcgAAEwEADk15LldlYlNlcnZpY2VzAAAAtAAAAM7K774BAAAAkQAAAGxTeXN0ZW0u
UmVzb3VyY2VzLlJlc291cmNlUmVhZGVyLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0
dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkjU3lzdGVtLlJlc291
cmNlcy5SdW50aW1lUmVzb3VyY2VTZXQCAAAAAAAAAAAAAABQQURQQURQtAAAAGhCAAAAAAAAAAAA
AIJCAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB0QgAAAAAAAAAAAAAAAF9Db3JEbGxNYWluAG1z
Y29yZWUuZGxsAAAAAAD/JQAgABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAEAAA
ABgAAIAAAAAAAAAAAAAAAAAAAAEAAQAAADAAAIAAAAAAAAAAAAAAAAAAAAEAAAAAAEgAAABYYAAA
UAQAAAAAAAAAAAAAUAQ0AAAAVgBTAF8AVgBFAFIAUwBJAE8ATgBfAEkATgBGAE8AAAAAAL0E7/4A
AAEAAAABAAAAAAAAAAEAAAAAAD8AAAAAAAAABAAAAAIAAAAAAAAAAAAAAAAAAABEAAAAAQBWAGEA
cgBGAGkAbABlAEkAbgBmAG8AAAAAACQABAAAAFQAcgBhAG4AcwBsAGEAdABpAG8AbgAAAAAAAACw
BLADAAABAFMAdAByAGkAbgBnAEYAaQBsAGUASQBuAGYAbwAAAIwDAAABADAAMAAwADAAMAA0AGIA
MAAAABoAAQABAEMAbwBtAG0AZQBuAHQAcwAAAAAAAAA0AAoAAQBDAG8AbQBwAGEAbgB5AE4AYQBt
AGUAAAAAAE0AaQBjAHIAbwBzAG8AZgB0AAAAfgArAAEARgBpAGwAZQBEAGUAcwBjAHIAaQBwAHQA
aQBvAG4AAAAAAFMAVABfADgANwA5AGUAMABjADgAMgA2ADAANgA0ADQANwA0AGEAYQA3ADEANwBk
ADAAMwBlADAANABkADUANgBjADIAOQAuAHYAYgBwAHIAbwBqAAAAAAAwAAgAAQBGAGkAbABlAFYA
ZQByAHMAaQBvAG4AAAAAADEALgAwAC4AMAAuADAAAAB+AC8AAQBJAG4AdABlAHIAbgBhAGwATgBh
AG0AZQAAAFMAVABfADgANwA5AGUAMABjADgAMgA2ADAANgA0ADQANwA0AGEAYQA3ADEANwBkADAA
MwBlADAANABkADUANgBjADIAOQAuAHYAYgBwAHIAbwBqAC4AZABsAGwAAAAAAFoAGwABAEwAZQBn
AGEAbABDAG8AcAB5AHIAaQBnAGgAdAAAAEMAbwBwAHkAcgBpAGcAaAB0ACAAQAAgAE0AaQBjAHIA
bwBzAG8AZgB0ACAAMgAwADEAMAAAAAAAKgABAAEATABlAGcAYQBsAFQAcgBhAGQAZQBtAGEAcgBr
AHMAAAAAAAAAAACGAC8AAQBPAHIAaQBnAGkAbgBhAGwARgBpAGwAZQBuAGEAbQBlAAAAUwBUAF8A
OAA3ADkAZQAwAGMAOAAyADYAMAA2ADQANAA3ADQAYQBhADcAMQA3AGQAMAAzAGUAMAA0AGQANQA2
AGMAMgA5AC4AdgBiAHAAcgBvAGoALgBkAGwAbAAAAAAAdgArAAEAUAByAG8AZAB1AGMAdABOAGEA
bQBlAAAAAABTAFQAXwA4ADcAOQBlADAAYwA4ADIANgAwADYANAA0ADcANABhAGEANwAxADcAZAAw
ADMAZQAwADQAZAA1ADYAYwAyADkALgB2AGIAcAByAG8AagAAAAAANAAIAAEAUAByAG8AZAB1AGMA
dABWAGUAcgBzAGkAbwBuAAAAMQAuADAALgAwAC4AMAAAADgACAABAEEAcwBzAGUAbQBiAGwAeQAg
AFYAZQByAHMAaQBvAG4AAAAxAC4AMAAuADAALgAwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAADAAAAJQyAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==</BinaryItem>
        </ScriptProject>
      </DTS:ObjectData>
    </DTS:Executable>
  </DTS:Executables>
  <DTS:PackageVariables>
    <DTS:PackageVariable
      DTS:CreationName=""
      DTS:DTSID="{B3FEB483-8D15-4F9B-8E8E-DA95C3622B53}"
      DTS:Namespace="dts-designer-1.0"
      DTS:ObjectName="package-diagram">
      <DTS:Property
        DTS:DataType="8"
        DTS:Name="PackageVariableValue">&lt;Package xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:dwd="http://schemas.microsoft.com/DataWarehouse/Designer/1.0"&gt;&lt;dwd:DtsControlFlowDiagram&gt;&lt;dwd:BoundingLeft&gt;265&lt;/dwd:BoundingLeft&gt;&lt;dwd:BoundingTop&gt;1000&lt;/dwd:BoundingTop&gt;&lt;dwd:Layout&gt;&lt;dds&gt;&lt;diagram fontclsid="{0BE35203-8F91-11CE-9DE3-00AA004BB851}" mouseiconclsid="{0BE35204-8F91-11CE-9DE3-00AA004BB851}" defaultlayout="Microsoft.DataWarehouse.Layout.GraphLayout110" defaultlineroute="Microsoft.DataWarehouse.Layout.GraphLayout110" version="7" nextobject="8" scale="100" pagebreakanchorx="0" pagebreakanchory="0" pagebreaksizex="0" pagebreaksizey="0" scrollleft="0" scrolltop="0" gridx="150" gridy="150" marginx="1000" marginy="1000" zoom="100" x="19923" y="18283" backcolor="15334399" defaultpersistence="2" PrintPageNumbersMode="3" PrintMarginTop="0" PrintMarginBottom="635" PrintMarginLeft="0" PrintMarginRight="0" marqueeselectionmode="1" mousepointer="0" snaptogrid="0" autotypeannotation="1" showscrollbars="0" viewpagebreaks="0" donotforceconnectorsbehindshapes="1" backpictureclsid="{00000000-0000-0000-0000-000000000000}"&gt;&lt;font&gt;&lt;ddsxmlobjectstreamwrapper binary="01010000900180380100065461686f6d61" /&gt;&lt;/font&gt;&lt;mouseicon&gt;&lt;ddsxmlobjectstreamwrapper binary="6c74000000000000" /&gt;&lt;/mouseicon&gt;&lt;/diagram&gt;&lt;layoutmanager&gt;&lt;ddsxmlobj /&gt;&lt;/layoutmanager&gt;&lt;ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge.2" tooltip="Transfer Pictures" left="6033" top="1104" logicalid="3" controlid="1" masterid="0" hint1="0" hint2="0" width="3598" height="1164" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0"&gt;&lt;control&gt;&lt;ddsxmlobjectstreaminitwrapper binary="000800000e0e00008c040000" /&gt;&lt;/control&gt;&lt;layoutobject&gt;&lt;ddsxmlobj&gt;&lt;property name="LogicalObject" value="Package\Transfer Pictures" vartype="8" /&gt;&lt;property name="ShowConnectorSource" value="0" vartype="2" /&gt;&lt;/ddsxmlobj&gt;&lt;/layoutobject&gt;&lt;shape groupshapeid="0" groupnode="0" /&gt;&lt;/ddscontrol&gt;&lt;ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge.2" tooltip="Transfer Person Data" left="265" top="1000" logicalid="4" controlid="2" masterid="0" hint1="0" hint2="0" width="3598" height="1164" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0"&gt;&lt;control&gt;&lt;ddsxmlobjectstreaminitwrapper binary="000800000e0e00008c040000" /&gt;&lt;/control&gt;&lt;layoutobject&gt;&lt;ddsxmlobj&gt;&lt;property name="LogicalObject" value="Package\Transfer Person Data" vartype="8" /&gt;&lt;property name="ShowConnectorSource" value="0" vartype="2" /&gt;&lt;/ddsxmlobj&gt;&lt;/layoutobject&gt;&lt;shape groupshapeid="0" groupnode="0" /&gt;&lt;/ddscontrol&gt;&lt;/dds&gt;&lt;/dwd:Layout&gt;&lt;/dwd:DtsControlFlowDiagram&gt;&lt;/Package&gt;</DTS:Property>
    </DTS:PackageVariable>
    <DTS:PackageVariable
      DTS:CreationName=""
      DTS:DTSID="{BE7F4D8F-72D3-4A32-8EBA-9B00203FAEA0}"
      DTS:Namespace="dts-designer-1.0"
      DTS:ObjectName="package-diagram">
      <DTS:Property
        DTS:DataType="8"
        DTS:Name="PackageVariableValue">&lt;Package xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:dwd="http://schemas.microsoft.com/DataWarehouse/Designer/1.0"&gt;&lt;dwd:DtsControlFlowDiagram&gt;&lt;dwd:BoundingLeft&gt;265&lt;/dwd:BoundingLeft&gt;&lt;dwd:BoundingTop&gt;1000&lt;/dwd:BoundingTop&gt;&lt;dwd:Layout&gt;&lt;dds&gt;&lt;diagram fontclsid="{0BE35203-8F91-11CE-9DE3-00AA004BB851}" mouseiconclsid="{0BE35204-8F91-11CE-9DE3-00AA004BB851}" defaultlayout="Microsoft.DataWarehouse.Layout.GraphLayout110" defaultlineroute="Microsoft.DataWarehouse.Layout.GraphLayout110" version="7" nextobject="8" scale="100" pagebreakanchorx="0" pagebreakanchory="0" pagebreaksizex="0" pagebreaksizey="0" scrollleft="0" scrolltop="0" gridx="150" gridy="150" marginx="1000" marginy="1000" zoom="100" x="19923" y="18283" backcolor="15334399" defaultpersistence="2" PrintPageNumbersMode="3" PrintMarginTop="0" PrintMarginBottom="635" PrintMarginLeft="0" PrintMarginRight="0" marqueeselectionmode="1" mousepointer="0" snaptogrid="0" autotypeannotation="1" showscrollbars="0" viewpagebreaks="0" donotforceconnectorsbehindshapes="1" backpictureclsid="{00000000-0000-0000-0000-000000000000}"&gt;&lt;font&gt;&lt;ddsxmlobjectstreamwrapper binary="01010000900180380100065461686f6d61" /&gt;&lt;/font&gt;&lt;mouseicon&gt;&lt;ddsxmlobjectstreamwrapper binary="6c74000000000000" /&gt;&lt;/mouseicon&gt;&lt;/diagram&gt;&lt;layoutmanager&gt;&lt;ddsxmlobj /&gt;&lt;/layoutmanager&gt;&lt;ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge.2" tooltip="Transfer Pictures" left="6033" top="1104" logicalid="3" controlid="1" masterid="0" hint1="0" hint2="0" width="3598" height="1164" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0"&gt;&lt;control&gt;&lt;ddsxmlobjectstreaminitwrapper binary="000800000e0e00008c040000" /&gt;&lt;/control&gt;&lt;layoutobject&gt;&lt;ddsxmlobj&gt;&lt;property name="LogicalObject" value="Package\Transfer Pictures" vartype="8" /&gt;&lt;property name="ShowConnectorSource" value="0" vartype="2" /&gt;&lt;/ddsxmlobj&gt;&lt;/layoutobject&gt;&lt;shape groupshapeid="0" groupnode="0" /&gt;&lt;/ddscontrol&gt;&lt;ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge.2" tooltip="Transfer Person Data" left="265" top="1000" logicalid="4" controlid="2" masterid="0" hint1="0" hint2="0" width="3598" height="1164" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0"&gt;&lt;control&gt;&lt;ddsxmlobjectstreaminitwrapper binary="000800000e0e00008c040000" /&gt;&lt;/control&gt;&lt;layoutobject&gt;&lt;ddsxmlobj&gt;&lt;property name="LogicalObject" value="Package\Transfer Person Data" vartype="8" /&gt;&lt;property name="ShowConnectorSource" value="0" vartype="2" /&gt;&lt;/ddsxmlobj&gt;&lt;/layoutobject&gt;&lt;shape groupshapeid="0" groupnode="0" /&gt;&lt;/ddscontrol&gt;&lt;/dds&gt;&lt;/dwd:Layout&gt;&lt;/dwd:DtsControlFlowDiagram&gt;&lt;/Package&gt;</DTS:Property>
    </DTS:PackageVariable>
  </DTS:PackageVariables>
</DTS:Executable>