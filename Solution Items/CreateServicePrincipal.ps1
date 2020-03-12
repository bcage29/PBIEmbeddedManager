
$authResult = Connect-AzureAD

# AAD Application Name
$appDisplayName = "<application name>"
# PowerBI SP Security Group
$adSecurityGroupName = "<power bi service principal group name>" #

# get user account ID for logged in user
$user = Get-AzureADUser -ObjectId $authResult.Account.Id

# get tenant name of logged in user
$tenantId = $authResult.TenantId

# create new password credential for client secret
$appSecret = New-Guid
$startDate = Get-Date	
$passwordCredential = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordCredential
$passwordCredential.StartDate = $startDate
$passwordCredential.EndDate = $startDate.AddYears(1)
$passwordCredential.KeyId = New-Guid
$passwordCredential.Value = $appSecret 


Write-Host "Registering Azure AD Application named $appDisplayName in $tenantName"

# create Azure AD Application
$aadApplication = New-AzureADApplication `
                        -DisplayName $appDisplayName  `
                        -PublicClient $false `
                        -AvailableToOtherTenants $false `
                        -Homepage $replyUrl `
                        -PasswordCredentials $passwordCredential
                        

# create applicaiton's service principal
$appId = $aadApplication.AppId
$sp = New-AzureADServicePrincipal -AppId $appId
$spObjectId = $sp.ObjectId

# assign current user as owner
Add-AzureADApplicationOwner -ObjectId $aadApplication.ObjectId -RefObjectId $user.ObjectId

# Add service principal of the new app as member of Power BI Apps group
$adSecurityGroup = Get-AzureADGroup -Filter "DisplayName eq '$adSecurityGroupName'"
Add-AzureADGroupMember -ObjectId $($adSecurityGroup.ObjectId) -RefObjectId $($spObjectId)

Write-Host "TenantId: " $tenantId
Write-Host "ServicePrincipalId: " $appId
Write-Host "ServicePrincipalOid: " $spObjectId
Write-Host "ServicePrincipalKey: " $appSecret