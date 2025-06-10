import 'package:app_eventos/features/auth/widgets/fighter_detail_dialog.dart';
import 'package:flutter/material.dart';
import '../../../core/models/dto/fight_dto.dart';
import '../../../core/auth/tournament_service.dart';
import 'package:provider/provider.dart';
import '../../../core/auth/auth_provider.dart';

class FightsDialog extends StatelessWidget {
  final List<FightDto> fights;
  final int tournamentId;
  final bool showParticipateButton;

  const FightsDialog({
    super.key,
    required this.fights,
    required this.tournamentId,
    this.showParticipateButton = true,
  });

  @override
  Widget build(BuildContext context) {
    final user = Provider.of<AuthProvider>(context).user;

    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
      title: const Text('Peleas del Torneo'),
      content: SizedBox(
        width: double.maxFinite,
        child: fights.isEmpty
            ? const Text('No hay peleas registradas.')
            : ListView.separated(
                shrinkWrap: true,
                itemCount: fights.length,
                separatorBuilder: (_, __) => const SizedBox(height: 12),
                itemBuilder: (context, index) {
                  final f = fights[index];
                  final isFinished = f.status?.toLowerCase() == 'finalizada' || f.winnerId != null;
                  String? ganador;
                  if (f.winnerId != null) {
                    if (f.winnerId == f.fighter1Id) {
                      ganador = f.nombrePeleador1;
                    } else if (f.winnerId == f.fighter2Id) {
                      ganador = f.nombrePeleador2;
                    }
                  }
                  return Card(
                    color: isFinished ? Colors.green.shade50 : Colors.deepPurple.shade50,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                    elevation: 3,
                    child: Padding(
                      padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 16),
                      child: Row(
                        children: [
                          CircleAvatar(
                            backgroundColor: Colors.deepPurple.shade100,
                            child: const Icon(Icons.sports_mma, color: Colors.deepPurple),
                          ),
                          const SizedBox(width: 14),
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  '${f.nombrePeleador1 ?? 'Peleador 1'}',
                                  style: const TextStyle(
                                    fontWeight: FontWeight.bold,
                                    fontSize: 16,
                                  ),
                                ),
                                const SizedBox(height: 2),
                                Text(
                                  'vs',
                                  style: TextStyle(
                                    color: Colors.deepPurple.shade300,
                                    fontWeight: FontWeight.w500,
                                  ),
                                ),
                                const SizedBox(height: 2),
                                Text(
                                  '${f.nombrePeleador2 ?? 'Peleador 2'}',
                                  style: const TextStyle(
                                    fontWeight: FontWeight.bold,
                                    fontSize: 16,
                                  ),
                                ),
                                if (ganador != null) ...[
                                  const SizedBox(height: 8),
                                  Row(
                                    children: [
                                      const Icon(Icons.emoji_events, color: Colors.amber, size: 20),
                                      const SizedBox(width: 6),
                                      Text(
                                        'Ganador: $ganador',
                                        style: const TextStyle(
                                          color: Colors.amber,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ],
                                  ),
                                ],
                                const SizedBox(height: 8),
                                Row(
                                  children: [
                                    Icon(
                                      isFinished ? Icons.check_circle : Icons.schedule,
                                      color: isFinished ? Colors.green : Colors.deepPurple,
                                      size: 18,
                                    ),
                                    const SizedBox(width: 6),
                                    Text(
                                      f.status?.toUpperCase() ?? 'DESCONOCIDO',
                                      style: TextStyle(
                                        color: isFinished ? Colors.green : Colors.deepPurple,
                                        fontWeight: FontWeight.w600,
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
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
        if (showParticipateButton && user != null)
          ElevatedButton.icon(
            label: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
              child: Text('Participar'),
            ),
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.deepPurple,
              foregroundColor: Colors.white,
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            ),
            onPressed: () async {
              final success = await TournamentService().participarEnTorneo(user.userId!, tournamentId);
              if (context.mounted) {
                Navigator.pop(context);
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text(success
                        ? '¡Te has inscrito como participante!'
                        : 'No se pudo inscribir en el torneo'),
                  ),
                );
              }
            },
          ),
      ],
    );
  }
}