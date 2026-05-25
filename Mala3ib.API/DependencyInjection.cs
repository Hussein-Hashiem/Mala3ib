namespace Mala3ib.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthConfig(configuration)
                    .AddValidationConfig()
                    .AddBackGroundJobsConfig(configuration)
                    .AddMappingConfig();

            services.AddControllers();
            services.AddOpenApi();

            services.AddCors(options =>
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                )
            );

            var connectionString = configuration.GetConnectionString("defaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddDistributedMemoryCache();
            services.AddScoped<ICacheService, CacheService>();

            #region Repo
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IPlayerRepo, PlayerRepo>();
            services.AddScoped<IFieldRepo, FieldRepo>();
            services.AddScoped<IFieldOwnerRepo, FieldOwnerRepo>();
            services.AddScoped<IFieldSlotRepo, FieldSlotRepo>();
            services.AddScoped<IFollowRepo, FollowRepo>();
            services.AddScoped<IFieldReviewRepo, FieldReviewRepo>();
            services.AddScoped<IInvitationRepo, InvitationRepo>();
            #endregion

            #region Service
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<IFieldOwnerService, FieldOwnerService>();
            services.AddScoped<IFieldSlotService, FieldSlotService>();
            services.AddScoped<IFieldReviewService, FieldReviewService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IInvitationService, InvitationService>();
            #endregion

            services.AddHttpContextAccessor();
            services.AddExceptionHandler<GlobaExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Mail Service
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddScoped<EmailBodyBuilder>();

            var jwtSettings = configuration.GetSection(JwtOptions.Name).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static IServiceCollection AddValidationConfig(this IServiceCollection services)
        {
            // Diffrent Assembly
            services.AddBLLValidation();
            services.AddFluentValidationAutoValidation();
            return services;
        }
        public static IServiceCollection AddMappingConfig(this IServiceCollection services)
        {
            // Diffrent Assembly
            services.AddBLLMapping();
            return services;
        }
        public static IServiceCollection AddBackGroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();

            return services;
        }
    }
}
