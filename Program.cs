var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<CustomerBeneficiaryManagement.Logging.ApiLogger>();

builder.Services.AddScoped<CustomerBeneficiaryManagement.DataAccess.CustomerRepository>();
builder.Services.AddScoped<CustomerBeneficiaryManagement.Services.BeneficiaryService>();
builder.Services.AddScoped<CustomerBeneficiaryManagement.Services.CustomerService>();
builder.Services.AddScoped<CustomerBeneficiaryManagement.DataAccess.BeneficiaryRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
