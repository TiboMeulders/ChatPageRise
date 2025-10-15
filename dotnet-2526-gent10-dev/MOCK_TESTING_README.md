# Mock Authentication Testing

Deze configuratie is ingesteld om de inlogflow te testen met mockdata zonder een echte server.

## Test Accounts

De volgende test accounts zijn beschikbaar:

| Email | Wachtwoord | Rol |
|-------|-----------|-----|
| admin@nodo.be | password123 | Administrator |
| user@nodo.be | user123 | User |
| test@test.be | test | User |

## Hoe te testen:

1. Start de applicatie
2. Ga naar de homepage (/) - je wordt automatisch doorgestuurd naar /login
3. Log in met een van de bovenstaande accounts
4. Na succesvol inloggen word je doorgestuurd naar de homepage met MainLayout (header + navbar)

## Switching tussen Mock en Production:

### Voor Mock Testing (huidige configuratie):
De mockdata configuratie staat momenteel aan in `Program.cs`. De echte authenticatie code staat in commentaar.

### Voor Production gebruik:
1. Comment uit de mock configuratie sectie in `Program.cs`
2. Uncomment de production authentication sectie
3. Uncomment de HTTP client configuratie

## Login Flow:

1. **Niet ingelogd**: AuthorizeView op homepage detecteert dat je niet geautoriseerd bent
2. **Redirect**: RedirectToLogin component stuurt je door naar /login
3. **Login**: Vul credentials in en klik login
4. **Success**: Na succesvolle login word je doorgestuurd naar homepage
5. **Homepage**: Toont welkom bericht met MainLayout (header + navbar)

## Logout:

Je kunt uitloggen via de logout component, waarna je opnieuw wordt doorgestuurd naar de login pagina.
