import 'package:flutter/material.dart';
import '../../../core/auth/auth_service.dart';
import '../../../core/models/dto/fighter_for_friend_dto.dart';

class FightersScreen extends StatelessWidget {
  const FightersScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<FighterForFriendDto>>(
      future: AuthService().getAllFighters(),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        }
        final fighters = snapshot.data ?? [];
        if (fighters.isEmpty) {
          return const Center(child: Text('No hay peleadores registrados.'));
        }
        return ListView.separated(
          padding: const EdgeInsets.all(16),
          itemCount: fighters.length,
          separatorBuilder: (_, __) => const SizedBox(height: 16),
          itemBuilder: (context, index) {
            final f = fighters[index];
            return Card(
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(18),
              ),
              elevation: 6,
              shadowColor: Colors.deepPurple.withOpacity(0.15),
              child: Padding(
                padding: const EdgeInsets.all(18),
                child: Row(
                  children: [
                    CircleAvatar(
                      radius: 32,
                      backgroundColor: Colors.deepPurple.shade100,
                      child: const Icon(Icons.sports_mma, color: Colors.deepPurple, size: 32),
                    ),
                    const SizedBox(width: 18),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            '${f.name} ${f.lastName}',
                            style: const TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 18,
                              color: Colors.deepPurple,
                            ),
                          ),
                          const SizedBox(height: 4),
                          Text(
                            '@${f.username}',
                            style: TextStyle(
                              color: Colors.deepPurple.shade300,
                              fontSize: 14,
                            ),
                          ),
                          const SizedBox(height: 8),
                          Row(
                            children: [
                              const SizedBox(width: 4),
                              Text(f.weightClass, style: const TextStyle(fontSize: 15)),
                              if (f.height != null) ...[
                                const SizedBox(width: 12),
                                const Icon(Icons.height, size: 18, color: Colors.deepPurple),
                                const SizedBox(width: 2),
                                Text('${f.height} cm', style: const TextStyle(fontSize: 15)),
                              ],
                              if (f.reach != null) ...[
                                const SizedBox(width: 12),
                                const Icon(Icons.open_with, size: 18, color: Colors.deepPurple),
                                const SizedBox(width: 2),
                                Text('${f.reach} cm', style: const TextStyle(fontSize: 15)),
                              ],
                            ],
                          ),
                          const SizedBox(height: 8),
                          Row(
                            children: [
                              _StatChip(label: 'W', value: f.wins, color: Colors.green),
                              const SizedBox(width: 6),
                              _StatChip(label: 'L', value: f.losses, color: Colors.red),
                              const SizedBox(width: 6),
                              _StatChip(label: 'D', value: f.draws, color: Colors.orange),
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
        );
      },
    );
  }
}

class _StatChip extends StatelessWidget {
  final String label;
  final int value;
  final Color color;

  const _StatChip({
    required this.label,
    required this.value,
    required this.color,
  });

  @override
  Widget build(BuildContext context) {
    return Chip(
      label: Text('$label: $value', style: TextStyle(color: Colors.white)),
      backgroundColor: color,
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 0),
    );
  }
}