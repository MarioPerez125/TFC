import 'package:app_eventos/core/api/endpoints.dart';
import 'package:app_eventos/core/models/dto/fight_dto.dart';
import 'package:app_eventos/core/models/dto/fighter_dto.dart';

import '../api/api_client.dart';
import '../models/dto/tournament_dto.dart';

class TournamentService {
  final ApiClient _apiClient = ApiClient();

  Future<List<TournamentDto>> getAllTournaments() async {
    final response = await _apiClient.get('/tournaments', requiresAuth: false);
    print('Respuesta torneos: ${response.data}'); // <-- Añade esto

    // Si la respuesta es una lista directa:
    if (response.statusCode == 200 && response.data is List) {
      return (response.data as List)
          .map((e) => TournamentDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }

    // Si la respuesta es un objeto con la lista bajo una clave:
    if (response.data is Map && response.data['tournament'] is List) {
      return (response.data['tournament'] as List)
          .map((e) => TournamentDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }

    return [];
  }

  Future<List<FighterDto>> getParticipants(int tournamentId) async {
    final response = await _apiClient.get(
      '/tournaments/$tournamentId/participants',
      requiresAuth: false,
    );
    if (response.statusCode == 200 && response.data is List) {
      return (response.data as List)
          .map((e) => FighterDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }
    return [];
  }

  Future<TournamentDto?> createTournament(TournamentDto dto) async {
    print('Llamando a crear torneo...');
    final response = await _apiClient.post(
      Endpoints.createTournament,
      body: dto.toJson(),
      requiresAuth: false,
    );
    print('Respuesta crear torneo: ${response.statusCode} ${response.data}');
    if (response.statusCode == 200 && response.data != null) {
      return TournamentDto.fromJson(response.data);
    }
    return null;
  }

  Future<List<FightDto>> getFightsByTournament(int tournamentId) async {
    final response = await _apiClient.get(
      'fights/by-tournament/$tournamentId',
      requiresAuth: false,
    );
    if (response.statusCode == 200 && response.data is List) {
      return (response.data as List)
          .map((e) => FightDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }
    // Si el backend devuelve un objeto con la lista bajo una clave, ajusta aquí
    if (response.data is Map && response.data['fights'] is List) {
      return (response.data['fights'] as List)
          .map((e) => FightDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }
    return [];
  }

  Future<bool> participarEnTorneo(int userId, int tournamentId) async {
    final response = await _apiClient.post(
      'tournaments/participar-en-torneo',
      body: {
        'participanteId': 0, // o null si el backend lo permite
        'userId': userId,
        'tournamentId': tournamentId,
      },
      requiresAuth: true,
    );
    return response.statusCode == 200;
  }
}
