import 'package:flutter/material.dart';
import '../../../core/models/dto/fighter_dto.dart';

class ParticipantsDialog extends StatelessWidget {
  final List<FighterDto> fighters;
  const ParticipantsDialog({super.key, required this.fighters});

  @override
  Widget build(BuildContext context) {
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
                        'Victorias: ${f.wins}  Derrotas: ${f.losses}  Empates: ${f.draws}'),
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
  }
}