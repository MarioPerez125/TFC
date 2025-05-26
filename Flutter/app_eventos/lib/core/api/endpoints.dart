class Endpoints {
  // URL base - debería venir de tu configuración de entorno
  static const String baseUrl = 'http://10.0.2.2/api';

  // Endpoints de Autenticación
  static const String login = '$baseUrl/auth/login';
  static const String register = '$baseUrl/auth/register';
  static const String registerAsOrganizer = '$baseUrl/auth/register-as-organizer';
  
  // Endpoints de Torneos
  static const String tournaments = '$baseUrl/tournaments';
  static String tournamentById(int id) => '$tournaments/$id';
  static const String upcomingTournaments = '$tournaments/upcoming';
  
  // Endpoints de Peleas
  static const String fights = '$baseUrl/fights';
  static String fightsByTournament(int tournamentId) => '$fights/tournament/$tournamentId';
  
  // Endpoints de Resultados
  static const String fightResults = '$baseUrl/fight-results';
  static String fightResultById(int id) => '$fightResults/$id';
  
  // Endpoints de Usuarios
  static const String users = '$baseUrl/users';
  static String userById(int id) => '$users/$id';
  
  // Endpoints de Fighters
  static const String fighters = '$baseUrl/fighters';
  static String fighterById(int id) => '$fighters/$id';
  static String registerToTournament(int tournamentId) => '$fighters/register/$tournamentId';
}