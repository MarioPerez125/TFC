import 'package:app_eventos/features/auth/widgets/fights_dialog.dart';
import 'package:flutter/material.dart';
import '../../../core/models/dto/tournament_dto.dart';
import '../../../core/auth/tournament_service.dart';
import '../../../core/models/dto/fighter_dto.dart';
import '../../../core/models/dto/fight_dto.dart';

class TournamentCard extends StatelessWidget {
  final TournamentDto tournament;
  const TournamentCard({super.key, required this.tournament});

  String _formatDate(String dateStr) {
    try {
      final date = DateTime.parse(dateStr);
      return '${date.day.toString().padLeft(2, '0')}/'
          '${date.month.toString().padLeft(2, '0')}/'
          '${date.year} ${date.hour.toString().padLeft(2, '0')}:'
          '${date.minute.toString().padLeft(2, '0')}';
    } catch (_) {
      return dateStr;
    }
  }

  void _showParticipants(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) {
        return FutureBuilder<List<FighterDto>>(
          future: TournamentService().getParticipants(tournament.tournamentId),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const AlertDialog(
                content: SizedBox(
                  height: 80,
                  child: Center(child: CircularProgressIndicator()),
                ),
              );
            }
            final fighters = snapshot.data ?? [];
            return AlertDialog(
              title: const Text('Participantes'),
              content: SizedBox(
                width: double.maxFinite,
                child: fighters.isEmpty
                    ? const Text('No hay participantes.')
                    : ListView.builder(
                        shrinkWrap: true,
                        itemCount: fighters.length,
                        itemBuilder: (context, index) {
                          final f = fighters[index];
                          return ListTile(
                            leading: const Icon(Icons.person),
                            title: Text('ID: ${f.userId}'),
                            subtitle: Text(
                              'Peso: ${f.weightClass} | Altura: ${f.height}cm | Alcance: ${f.reach}cm\n'
                              'Victorias: ${f.wins}  Derrotas: ${f.losses}  Empates: ${f.draws}',
                            ),
                          );
                        },
                      ),
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(context),
                  child: const Text('Cerrar'),
                ),
              ],
            );
          },
        );
      },
    );
  }

  void _showFights(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) {
        return FutureBuilder<List<FightDto>>(
          future: TournamentService().getFightsByTournament(tournament.tournamentId),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const AlertDialog(
                content: SizedBox(
                  height: 80,
                  child: Center(child: CircularProgressIndicator()),
                ),
              );
            }
            final fights = snapshot.data ?? [];
            return FightsDialog(fights: fights, tournamentId: tournament.tournamentId);
          },
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return InkWell(
      borderRadius: BorderRadius.circular(20),
      onTap: () => _showFights(context),
      child: Card(
        margin: const EdgeInsets.symmetric(vertical: 12, horizontal: 10),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        elevation: 6,
        shadowColor: Colors.deepPurple.withOpacity(0.2),
        child: Padding(
          padding: const EdgeInsets.all(18),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Icon(Icons.sports_mma, color: Colors.deepPurple, size: 28),
                  const SizedBox(width: 10),
                  Text(
                    tournament.location,
                    style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                      color: Colors.deepPurple,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 8),
              Text(
                'Arena: ${tournament.arena}',
                style: const TextStyle(fontWeight: FontWeight.w500),
              ),
              const SizedBox(height: 8),
              Text(
                'Deporte: ${tournament.sportType}',
                style: const TextStyle(fontWeight: FontWeight.w500),
              ),
              const SizedBox(height: 8),
              Row(
                children: [
                  const Icon(
                    Icons.calendar_today,
                    size: 18,
                    color: Colors.deepPurple,
                  ),
                  const SizedBox(width: 4),
                  Text('Inicio: ${_formatDate(tournament.startDate)}'),
                ],
              ),
              const SizedBox(height: 4),
              Row(
                children: [
                  const Icon(Icons.flag, size: 18, color: Colors.deepPurple),
                  const SizedBox(width: 4),
                  Text('Fin: ${_formatDate(tournament.endDate)}'),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
