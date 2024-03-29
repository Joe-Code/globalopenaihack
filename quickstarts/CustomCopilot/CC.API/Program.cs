using CC.API;
using Hl7.Fhir.Model;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string apiKey = builder.Configuration["OpenAI:ApiKey"] ??
    throw new ApplicationException("No OpenAI API key found in configuration.");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

RouteGroupBuilder patientApis = app.MapGroup("/patients");
patientApis.MapGet("/", GetAllPatients);
patientApis.MapGet("/{id}", GetPatientById);
patientApis.MapPost("/generate/patients", GeneratePatients);
patientApis.MapPost("/generate/encounters", GenerateEncounters);

app.Run();

async Task<IResult> GeneratePatients()
{
    AiHelper aiHelper = new(apiKey);
    List<string> characters = await aiHelper.GeneratePatients();

    foreach (string character in characters)
    {
        await aiHelper.GenerateFhirOfPatient(character);
    }

    return TypedResults.Ok();
}

async Task<IResult> GenerateEncounters()
{
    AiHelper aiHelper = new(apiKey);
    List<string> characters = await aiHelper.GeneratePatients();

    foreach (string character in characters)
    {
        await aiHelper.GenerateFhirEncounters(character);
    }

    return TypedResults.Ok();
}

static async Task<IResult> GetAllPatients()
{
    List<string> patients = new();

    string patientDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Patients");
    string[] files = Directory.GetFiles(patientDataPath);
    foreach (string file in files)
    {
        using StreamReader reader = new(file);
        string patient = await reader.ReadToEndAsync();

        if (patient is not null)
            patients.Add(patient);
    }

    return TypedResults.Ok(patients);
}

static IResult GetPatientById()
{
    Patient patient = new();

    return TypedResults.Ok(patient);
}