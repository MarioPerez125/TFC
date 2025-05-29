import 'package:app_eventos/core/auth/tournament_service.dart';
import 'package:app_eventos/features/auth/widgets/create_tournament_dialog';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../../core/auth/auth_provider.dart';
import '../../../core/models/dto/tournament_dto.dart';
import '../widgets/tournament_card.dart';

class TournamentsScreen extends StatefulWidget {
  const TournamentsScreen({super.key});

  @override
  State<TournamentsScreen> createState() => _TournamentsScreenState();
}

class _TournamentsScreenState extends State<TournamentsScreen> {
  late Future<List<TournamentDto>> _futureTournaments;

  @override
  void initState() {
    super.initState();
    _futureTournaments = TournamentService().getAllTournaments();
  }

  Future<void> _refreshTournaments() async {
    setState(() {
      _futureTournaments = TournamentService().getAllTournaments();
    });
  }

  void _showCreateTournamentDialog(
    BuildContext context,
    int organizerId,
  ) async {
    final result = await showDialog<TournamentDto>(
      context: context,
      builder: (context) => CreateTournamentDialog(organizerId: organizerId),
    );
    if (result != null) {
      await TournamentService().createTournament(result);
      await _refreshTournaments();
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('¡Torneo creado con éxito!')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final user = Provider.of<AuthProvider>(context).user;

    return Scaffold(
      body: FutureBuilder<List<TournamentDto>>(
        future: _futureTournaments,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }
          final tournaments = snapshot.data ?? [];
          return ListView.builder(
            padding: const EdgeInsets.all(8),
            itemCount: user?.role == 'Organizer'
                ? tournaments.length + 1
                : tournaments.length,
            itemBuilder: (context, index) {
              if (index < tournaments.length) {
                return TournamentCard(tournament: tournaments[index]);
              }
              // Último elemento: el botón
              return Padding(
                padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 18),
                child: SizedBox(
                  width: double.infinity,
                  height: 56,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.deepPurple,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(18),
                      ),
                      elevation: 8,
                    ),
                    onPressed: () =>
                        _showCreateTournamentDialog(context, user!.userId!),
                    child: const Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [Icon(Icons.add, size: 28, color: Colors.white)],
                    ),
                  ),
                ),
              );
            },
          );
        },
      ),
    );
  }
}
