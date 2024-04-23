using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Sap.Data.Hana;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
//Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(mySQLConfiguration);

var odbcConfiguration = new ODBCConfiguration(builder.Configuration.GetConnectionString("ODBCConnection"));
builder.Services.AddSingleton(odbcConfiguration);

var hanaConnectionString = builder.Configuration.GetConnectionString("HanaConnection");
builder.Services.AddSingleton(new HanaConnection(hanaConnectionString));

//Inyección de Path
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

//builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));
builder.Services.AddScoped<IAcquisitionBrandRepository, AcquisitionBrandRepository>();
builder.Services.AddScoped<IAcquisitionCarrierRepository, AcquisitionCarrierRepository>();
builder.Services.AddScoped<IAcquisitionCategoryRepository, AcquisitionCategoryRepository>();
builder.Services.AddScoped<IAcquisitionEntryBySupplierHRepository, AcquisitionEntryBySupplierHRepository>();
builder.Services.AddScoped<IAcquisitionEntryBySupplierLRepository, AcquisitionEntryBySupplierLRepository>();
builder.Services.AddScoped<IAcquisitionEntryHRepository, AcquisitionEntryHRepository>();
builder.Services.AddScoped<IAcquisitionEntryLRepository, AcquisitionEntryLRepository>();
builder.Services.AddScoped<IAcquisitionItemByDepartmentRepository, AcquisitionItemByDepartmentRepository>();
builder.Services.AddScoped<IAcquisitionMeasurementRepository, AcquisitionMeasurementRepository>();
builder.Services.AddScoped<IAcquisitionModelRepository, AcquisitionModelRepository>();
builder.Services.AddScoped<IAcquisitionProductRepository, AcquisitionProductRepository>();
builder.Services.AddScoped<IAcquisitionReleaseHRepository, AcquisitionReleaseHRepository>();
builder.Services.AddScoped<IAcquisitionReleaseLRepository, AcquisitionReleaseLRepository>();
builder.Services.AddScoped<IAcquisitionSupplyRepository, AcquisitionSupplyRepository>();
builder.Services.AddScoped<IAcquisitionVehicleRepository, AcquisitionVehicleRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IBusinessnameRepository, BusinessnameRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
builder.Services.AddScoped<ISAPAssetRetirementRepository, SAPAssetRetirementRepository>();
builder.Services.AddScoped<ISAPFixedAssetRepository, SAPFixedAssetRepository>();
builder.Services.AddScoped<ISAPPurchaseInvoicesRepository, SAPPurchaseInvoicesRepository>();
builder.Services.AddScoped<ILoginRequestRepository, LoginRequestRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    policy =>
    {
        policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// Configurar la validación automática del modelo
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

var app = builder.Build();
app.UseCors();




// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
