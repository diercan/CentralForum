Imports Janus.Windows.Ribbon
Imports CentralNamespace = Central.Icons
Imports Central.Icons
Imports MYOB.CSS
Imports CentralForum.Client
Imports System.Windows.Forms.Integration
Imports CentralForum.Client.Forum
Imports CentralForum.Client.Model.Entities
Imports CentralForum.Client.Model
''' <summary>
''' This is the base class for forms that want to live inside
''' Central(TM)'s main tab area.
''' Prepare for some hoop-jumping to get Central(TM) to do what we
''' want it to do...
''' </summary>
''' <remarks></remarks>
Public Class TaxMainAreaForm
    Implements MYOB.CSSInterface.ICSSDisplayMainArea

    Private WithEvents _sideBarManager As SideBarManager
    Private _killedByFramework As Boolean
    Private _binder As CSSFormBinder
    Private _explorerBar As Janus.Windows.ExplorerBar.ExplorerBar
    Private _panelManager As CCH.UKTax.Janus4Controls.PanelManager.UIPanelManager = New CCH.UKTax.Janus4Controls.PanelManager.UIPanelManager(Me.components)
    Private _panelForum As CCH.UKTax.Janus4Controls.PanelManager.UIPanelBase = New CCH.UKTax.Janus4Controls.PanelManager.UIPanelBase()
    Private _group As New CCH.UKTax.Janus4Controls.PanelManager.UIPanelGroup()
    Private _forumControl As Forum.ForumView
    Private _wpfHost As ElementHost

#Region " Constructor(s)"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyBase.TopLevel = False

        ApplyPaddingForRibbon()

    End Sub

#End Region

    Private Sub InitForum()

        'keep these three in the current order
        _panelManager.Panels.Add(_group)
        _group.Panels.Add(_panelForum)

        _forumControl = New Forum.ForumView()
        Dim vm As New ForumViewModel(New DataService(), LoadForumContext())
        _forumControl.DataContext = vm

        ''Me.TaxReturn
        _wpfHost = New ElementHost()
        _wpfHost.Child = _forumControl
        _wpfHost.Dock = DockStyle.Fill
        ''AddHandler Me.taxReturnWorkflows.OnPinButtonClicked, AddressOf PinButtonClicked

        _panelForum.Text = "Forum"
        _panelForum.CaptionVisible = False
        _panelForum.InnerContainer.Controls.Add(Me._wpfHost)

        _group.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.Tab
        _group.DockStyle = Janus.Windows.UI.Dock.PanelDockStyle.Right

        _panelManager.AllowPanelDrag = False
        _panelManager.DefaultPanelSettings.AutoHideTabDisplay = Janus.Windows.UI.Dock.TabDisplayMode.ImageAndText
        _panelManager.ContainerControl = Me

    End Sub

    Const LoadUserContextQuery As String = "
SELECT 
	c.RowGuid,
	c.LNameFName,
	(SELECT TOP 1 p.PracticeGuid FROM dbo.Practice p) practiceGuid,
	(SELECT TOP 1 c.KeyValue FROM dbo.Configuration c WHERE c.Keyname = 'Portal_PortalSubDomain') PracticeName
FROM dbo.Employee e
	INNER JOIN dbo.Contact c on c.ContactID = e.ContactID
WHERE e.EmployeeID=@employeeId"

    Private Function LoadForumContext() As ForumContext
        If Not TaxUI.Handshake() Then
            Return Nothing
        End If

        Dim context As New ForumContext()
        Dim query As New TaxDataQuery()
        query.SQL = LoadUserContextQuery
        query.Parameters.Add("@employeeId", CssContext.Instance.CurrentEmployeeId)
        Dim data As DataSet = TaxUI.Connection.RunQuery(query)
        Dim row As DataRow = data.Tables(0).Rows(0)

        context.UserId = DirectCast(row("RowGuid"), Guid)
        context.UserDisplayName = GetString(row, "LNameFName")
        context.PracticeDisplayName = GetString(row, "PracticeName")
        context.PracticeId = DirectCast(row("practiceGuid"), Guid)

        Dim sourceNode As SourceNode = GetSourceNodeForControl(Me)
        If (sourceNode IsNot Nothing) Then
            context.TopicDisplayName = sourceNode.Description
            context.TopicName = "PT-" + sourceNode.Type.ToString()
        Else
            context.TopicDisplayName = "Personal Tax"
            context.TopicName = "PT"
        End If
        Return context
    End Function


#Region " Protected methods"
    Protected ReadOnly Property KilledByFramework() As Boolean
        Get
            Return Me._killedByFramework
        End Get
    End Property
#End Region

#Region " Overridable methods"

    ''' <summary>
    ''' Navigate to a specified location
    ''' </summary>
    ''' <param name="NavProxy"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub NavigateTo(ByVal NavProxy As NavigationProxy)

    End Sub

    Protected Overridable Sub SetBinder(ByVal Binder As CSSFormBinder)
        Me._binder = Binder
        Me._binder.SetForm(Me)
    End Sub

    ''' <summary>
    ''' This method is called to create the side bar icons
    ''' for the form.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub CreateSideBarItems()
        If TaxUI.IsTaxDeveloper = True AndAlso MYOB.CSS.CssContext.Instance.Host.IsRibbonView() Then
            Me.SideBarGroup("Developer")
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperDatabase, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D1", 1)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperWhoWroteThis, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D2", 2)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperWhyIsThisDataDirty, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D3", 3)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGridDataView, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D4", 4)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperShowBinding, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D5", 5)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperFocusHunter, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D6", 6)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperEmulateCSSIssues, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D7", 7)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGarbageCollect, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D8", 8)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperRecalcList, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D9", 9)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperTRPHiddenFields, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D10", 10)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperImpex, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D11", 11)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperViewNoteCache, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D12", 12)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperViewFormList, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D13", 13)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperCalcView, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D14", 14)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperXmlClient, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D15", 15)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperFTCRData, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D16", 16)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperShowLocks, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D17", 17)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGenerateCentralReporting, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Developer", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "D18", 18)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MmenuDeveloperScreen1024x768, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Screen", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "S1", 19)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreen800x600, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Screen", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "S2", 20)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreenFullScreen, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Screen", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "S3", 21)
            Me.SideBarItem(UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreenDisplayCurrentTabSize, IconLibrary.Tick(), Janus.Windows.Ribbon.CommandSizeStyle.Small, "Screen", Nothing, Janus.Windows.Ribbon.CommandType.DropDownButton, "S4", 22)

        End If

    End Sub

    Protected ReadOnly Property IsTaxReturnReadOnly() As Boolean
        Get
            Return TaxReturnLocking.IsReadOnly
        End Get
    End Property

    ''' <summary>
    ''' A side bar item has been clicked
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSideBarItemClicked(ByVal Item As SideBarManager.SideBarItemTypes)
        Select Case Item
            Case SideBarManager.SideBarItemTypes.CloseWindow
                Me.Close()
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperDatabase
                DevOptions.MenuDeveloperDatabase(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperWhoWroteThis
                DevOptions.MenuDeveloperWhoWroteThis(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperWhyIsThisDataDirty
                DevOptions.MenuDeveloperWhyIsThisDataDirty(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGridDataView
                DevOptions.MenuDeveloperGridDataView(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperShowBinding
                DevOptions.MenuDeveloperShowBinding(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperFocusHunter
                DevOptions.MenuDeveloperFocusHunter(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperEmulateCSSIssues
                DevOptions.MenuDeveloperEmulateCSSIssues(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGarbageCollect
                DevOptions.MenuDeveloperGarbageCollect(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperRecalcList
                DevOptions.MenuDeveloperRecalcList(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperTRPHiddenFields
                DevOptions.MenuDeveloperTRPHiddenFields(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperImpex
                DevOptions.MenuDeveloperImpex(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperViewNoteCache
                DevOptions.MenuDeveloperViewNoteCache(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperViewFormList
                DevOptions.MenuDeveloperViewFormList(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperCalcView
                DevOptions.MenuDeveloperCalcView(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperXmlClient
                DevOptions.MenuDeveloperXmlClient(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperFTCRData
                DevOptions.MenuDeveloperFTCRData(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperShowLocks
                DevOptions.MenuDeveloperShowLocks(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperGenerateCentralReporting
                DevOptions.MenuDeveloperGenerateCentralReporting(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MmenuDeveloperScreen1024x768
                DevOptions.MmenuDeveloperScreen1024x768(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreen800x600
                DevOptions.MenuDeveloperScreen800x600(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreenFullScreen
                DevOptions.MenuDeveloperScreenFullScreen(Nothing, Nothing)
            Case UI.Framework.SideBarManager.SideBarItemTypes.MenuDeveloperScreenDisplayCurrentTabSize
                DevOptions.MenuDeveloperScreenDisplayCurrentTabSize(Nothing, Nothing)

        End Select
    End Sub

    ''' <summary>
    ''' Get the contact for this form
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Overridable ReadOnly Property Contact() As Contact
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Optional parameters have been specified during the invoke of the form
    ''' </summary>
    ''' <param name="Binder"></param>
    ''' <remarks></remarks>
    Protected Friend Overridable Sub BindOptionalParameters(ByVal Binder As CSSFormBinder)

    End Sub

    Protected Sub BindOptionalParameters()
        Me.BindOptionalParameters(Me._binder)
    End Sub

#End Region

#Region " Overrides"

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        InitForum()
    End Sub



    ''' <summary>
    ''' More jumping through hoops to get Central(TM) to set the
    ''' correct title when WE want to set it
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Me.UpdateTabCaption()
    End Sub

    ''' <summary>
    ''' If we have set the required tab text too late for Central(TM)
    ''' to use the one we want but too early for us to do it above,
    ''' hopefully this bit of hoop-jumping will ensure that we get it
    ''' done.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        MyBase.OnParentChanged(e)
        Me.UpdateTabCaption()
    End Sub

    ''' <summary>
    ''' Because of a problem with Central(TM) we cannot allow the
    ''' OnFormClosing event to fire (because it will dispose our form!)
    ''' Instead we avoid sending it until we are closed.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        'MyBase.OnFormClosing(e)
    End Sub

    ''' <summary>
    ''' Form is closed.
    ''' We need to fire the OnClosing event for Central(TM)
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnFormClosed(ByVal e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        MyBase.OnFormClosing(New System.Windows.Forms.FormClosingEventArgs(e.CloseReason, False))
        If Me._binder IsNot Nothing Then
            Me._binder.ReleaseLicenses()
            Me._binder.ReleaseLock()
        End If
    End Sub

#End Region

#Region " Friend methods"

    ''' <summary>
    ''' Start a new side bar group
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarGroup(ByVal Text As String)
        _sideBarManager.StartGroup(Text)
    End Sub

    ''' <summary>
    ''' Start a new side bar group
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarGroup(ByVal Text As String, ByVal TabName As String)
        _sideBarManager.StartGroup(Text, TabName)
    End Sub

    ''' <summary>
    ''' Add a new side bar item
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarItem(ByVal Item As SideBarManager.SideBarItemTypes)
        _sideBarManager.AddItem(Item)
    End Sub

    ''' <summary>
    ''' Add a new side bar item
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarItem(ByVal Item As SideBarManager.SideBarItemTypes, ByVal Icon As Icon, ByVal Size As CommandSizeStyle, ByVal KeyTip As String, ByVal ribbonPosition As Int32)
        _sideBarManager.AddItem(Item, Icon, Size, KeyTip, ribbonPosition)
    End Sub

    ''' <summary>
    ''' Add a new side bar item
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarItem(ByVal Item As SideBarManager.SideBarItemTypes, ByVal Icon As Icon, ByVal Size As CommandSizeStyle, ByVal GroupName As String, ByVal KeyTip As String, ByVal ribbonPosition As Int32)
        _sideBarManager.AddItem(Item, Icon, Size, GroupName, KeyTip, ribbonPosition)
    End Sub

    ''' <summary>
    ''' Add a new side bar item
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Protected Sub SideBarItem(ByVal Item As SideBarManager.SideBarItemTypes, ByVal Icon As Icon, ByVal Size As CommandSizeStyle, ByVal DropdownButtonText As String, ByVal DropdownButtonIcon As Icon,
                              ByVal CommandType As CommandType, ByVal KeyTip As String, ByVal ribbonPosition As Int32)
        _sideBarManager.AddItem(Item, Icon, Size, DropdownButtonText, DropdownButtonIcon, CommandType, KeyTip, ribbonPosition)
    End Sub

    ''' <summary>
    ''' Get side bar manager
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property SideBarManager() As SideBarManager
        Get
            Return _sideBarManager
        End Get
    End Property

    ''' <summary>
    ''' Enable or disable an item in the sidebar
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property SideBarItemEnabled(ByVal Item As SideBarManager.SideBarItemTypes) As Boolean
        Get
            Return Me._sideBarManager.Enable(Item)
        End Get
        Set(ByVal value As Boolean)
            Me._sideBarManager.Enable(Item) = value
        End Set
    End Property

    ''' <summary>
    ''' Make an item in the side bar visible/invisible
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property SideBarItemVisible(ByVal Item As SideBarManager.SideBarItemTypes) As Boolean
        Get
            Return Me._sideBarManager.Visible(Item)
        End Get
        Set(ByVal value As Boolean)
            Me._sideBarManager.Visible(Item) = value
        End Set
    End Property

    ''' <summary>
    ''' Close the form without the usual checks
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub Kill()
        Me._killedByFramework = True
        'Remove from Central
        MyBase.OnFormClosing(New FormClosingEventArgs(CloseReason.FormOwnerClosing, False))
        If Not Me._binder Is Nothing Then
            Me._binder.ReleaseLicenses()
            Me._binder.ReleaseLock()
        End If
        'Dispose
        Me.Dispose()
    End Sub

    Friend Function IsInstanceOf(ByVal Binder As CSSFormBinder) As Boolean
        Return Binder.Equals(_binder)
    End Function

#End Region

#Region " Event handlers"

    Private Sub _sideBarManager_Activate(ByVal Item As SideBarManager.SideBarItemTypes) Handles _sideBarManager.Activate
        Select Case Item
            Case SideBarManager.SideBarItemTypes.CreateHMRCOffice
                formCreateHMRCOffice.CreateOffice(Me)
            Case SideBarManager.SideBarItemTypes.EditCommonDescriptions, UI.Framework.SideBarManager.SideBarItemTypes.EditCommonDescriptionsUX
                Navigation.OpenForm(Of formCommonDescriptions)()
            Case SideBarManager.SideBarItemTypes.EditExpenseDescriptions, UI.Framework.SideBarManager.SideBarItemTypes.EditExpenseDescriptionsUX
                Navigation.OpenForm(Of formExpenseDescriptions)()
            Case Else
                Me.OnSideBarItemClicked(Item)
        End Select
    End Sub

#End Region

#Region " ICSSDisplayMainArea"

    Private Sub ApplyPaddingForRibbon()
        If MYOB.CSS.CssContext.Instance.Host IsNot Nothing AndAlso MYOB.CSS.CssContext.Instance.Host.IsRibbonView() Then
            Me.Padding = New Padding(10, 0, 10, 0)
        End If
    End Sub

    Private Sub CloseForm(ByVal sender As Object, ByVal e As CSSInterface.CSSCancelEventArgs) Implements CSSInterface.ICSSDisplayMainArea.CloseForm
        Me.Close()
        If Not Me.IsDisposed Then e.Cancel = True
    End Sub

    Private _collectionId As Integer
    Private Property CollectionID() As Integer Implements CSSInterface.ICSSDisplayMainArea.CollectionID
        Get
            Return _collectionId
        End Get
        Set(ByVal value As Integer)
            _collectionId = value
        End Set
    End Property

    Private Function Register() As CSSInterface.SideBarGroups Implements CSSInterface.ICSSDisplayMainArea.Register
        'Create side bar
        _explorerBar = New Janus.Windows.ExplorerBar.ExplorerBar()
        _sideBarManager = New SideBarManager(_explorerBar)
        SideBarGroup(CStr(IIf(MYOB.CSS.CssContext.Instance.Host.IsRibbonView(), "Common", "Window")))
        SideBarItem(SideBarManager.SideBarItemTypes.CloseWindow, CentralNamespace.IconLibrary.CloseWindow, CommandSizeStyle.Large, "C", 0)
        CreateSideBarItems()
        'Show the form
        Me.Show()
        'Return items
        Return _sideBarManager.ToSideBarGroups()
    End Function

#End Region

#Region " Tab captions"

    Protected Sub UpdateTabCaption()
        'check for full name since Central used Janus 4 and PT uses Janus 2
        If Me.Parent Is Nothing OrElse Me.Parent.GetType().FullName <> "Janus.Windows.UI.Tab.UITabPage" Then Exit Sub
        Me.Tag = UserPreferences.Current.TabCaptionStyle.GetText(Me)
        Me.Parent.Text = Tag.ToString()
    End Sub

    Public Shared Sub UpdateAllTabCaptions()
        For Each Form As TaxMainAreaForm In Framework.FindInstances(GetType(TaxMainAreaForm))
            Form.UpdateTabCaption()
        Next
    End Sub


#End Region

End Class
