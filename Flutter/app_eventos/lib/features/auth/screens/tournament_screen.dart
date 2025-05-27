import 'package:app_eventos/core/auth/tournament_service.dart';
import 'package:flutter/material.dart';
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

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<TournamentDto>>(
      future: _futureTournaments,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        }
        if (!snapshot.hasData || snapshot.data!.isEmpty) {
          return const Center(child: Text('No hay torneos disponibles.'));
        }
        return ListView.builder(
          padding: const EdgeInsets.all(8),
          itemCount: snapshot.data!.length,
          itemBuilder: (context, index) {
            return TournamentCard(tournament: snapshot.data![index]);
          },
        );
      },
    );
  }
}