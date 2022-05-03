using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using IdSrvr4Demo.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddIdentityServer(opt =>
{
  opt.Events.RaiseErrorEvents = true;
  opt.Events.RaiseInformationEvents = true;
  opt.Events.RaiseFailureEvents = true;
  opt.Events.RaiseSuccessEvents = true;
  opt.AccessTokenJwtType = "JWT";
  opt.EmitStaticAudienceClaim = true;

})
.AddDeveloperSigningCredential()  
    .AddTestUsers(Config.GetUsers())
    .AddInMemoryPersistedGrants()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients());

builder.Services.AddAuthentication()
   .AddOpenIdConnect("aad", "Azure AD", options =>
   {
     options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
     options.SignOutScheme = IdentityServerConstants.SignoutScheme;
     options.ClientSecret = "gch8Q~Nlmm3fK6zHXgz1zdrI4BICj34i5n9Lzcra";
    // options.SignInScheme = IdentityConstants.ApplicationScheme;
     options.ResponseType = OpenIdConnectResponseType.Code;
     options.ClientId = "fb445e1d-3d98-4976-9774-ef39d15f6ca2";
     options.Authority = "https://login.microsoftonline.com/6f3906f5-0b50-4caf-a4c7-b237d932de3b/";
     options.CallbackPath = "/signin-oidc";
    // options.UsePkce Authentication = true;
   });
   

var app = builder.Build();

app.UseIdentityServer();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
 {
   endpoints.MapDefaultControllerRoute();
 });

app.MapRazorPages();
app.Run();
