// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using SeekQ.Identity.Data;
using SeekQ.Identity.Models;
using SeekQ.Identity.Models.Profile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SeekQ.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    
                    context.Database.EnsureCreated();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("alice created");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    var male = context.UserGenders.Find(1);
                    if (male == null)
                    {
                        context.UserGenders.Add(new UserGender(1, "Male"));
                        context.SaveChanges();
                        Log.Debug("Male was created");
                    }
                    else
                    {
                        Log.Debug("Male already exists");
                    }

                    var female = context.UserGenders.Find(2);
                    if (female == null)
                    {
                        context.UserGenders.Add(new UserGender(2, "Female"));
                        context.SaveChanges();
                        Log.Debug("Female was created");
                    }
                    else
                    {
                        Log.Debug("Female already exists");
                    }

                    var agender = context.UserGenders.Find(3);
                    if (agender == null)
                    {
                        context.UserGenders.Add(new UserGender(3, "Agender"));
                        context.SaveChanges();
                        Log.Debug("Female was created");
                    }
                    else
                    {
                        Log.Debug("Female already exists");
                    }

                    var androgyne = context.UserGenders.Find(4);
                    if (androgyne == null)
                    {
                        context.UserGenders.Add(new UserGender(4, "Androgyne"));
                        context.SaveChanges();
                        Log.Debug("Androgyne was created");
                    }
                    else
                    {
                        Log.Debug("Androgyne already exists");
                    }

                    var trasgender = context.UserGenders.Find(5);
                    if (trasgender == null)
                    {
                        context.UserGenders.Add(new UserGender(5, "Trasgender"));
                        context.SaveChanges();
                        Log.Debug("Trasgender was created");
                    }
                    else
                    {
                        Log.Debug("Trasgender already exists");
                    }

                    var other = context.UserGenders.Find(6);
                    if (other == null)
                    {
                        context.UserGenders.Add(new UserGender(6, "Other"));
                        context.SaveChanges();
                        Log.Debug("Other was created");
                    }
                    else
                    {
                        Log.Debug("Other already exists");
                    }

                    var english = context.LanguageKnows.Find(1);
                    if (english == null)
                    {
                        context.LanguageKnows.Add(new LanguageKnow(1, "English"));
                        context.SaveChanges();
                        Log.Debug("English was created");
                    }
                    else
                    {
                        Log.Debug("English already exists");
                    }

                    var spanish = context.LanguageKnows.Find(2);
                    if (spanish == null)
                    {
                        context.LanguageKnows.Add(new LanguageKnow(2, "Spanish"));
                        context.SaveChanges();
                        Log.Debug("Spanish was created");
                    }
                    else
                    {
                        Log.Debug("Spanish already exists");
                    }

                    var german = context.LanguageKnows.Find(3);
                    if (german == null)
                    {
                        context.LanguageKnows.Add(new LanguageKnow(3, "German"));
                        context.SaveChanges();
                        Log.Debug("German was created");
                    }
                    else
                    {
                        Log.Debug("German already exists");
                    }

                    var french = context.LanguageKnows.Find(4);
                    if (french == null)
                    {
                        context.LanguageKnows.Add(new LanguageKnow(4, "French"));
                        context.SaveChanges();
                        Log.Debug("French was created");
                    }
                    else
                    {
                        Log.Debug("French already exists");
                    }

                    var italian = context.LanguageKnows.Find(5);
                    if (italian == null)
                    {
                        context.LanguageKnows.Add(new LanguageKnow(5, "Italian"));
                        context.SaveChanges();
                        Log.Debug("Italian was created");
                    }
                    else
                    {
                        Log.Debug("Italian already exists");
                    }
                }
            }
        }
    }
}
