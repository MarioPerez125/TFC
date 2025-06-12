import 'package:app_eventos/features/auth/widgets/fighter_detail_dialog.dart';
import 'package:flutter/material.dart';
import '../../../core/auth/service.dart';
import '../../../core/models/dto/fighter_for_friend_dto.dart';

class FightersScreen extends StatefulWidget {
  const FightersScreen({super.key});

  @override
  State<FightersScreen> createState() => _FightersScreenState();
}

class _FightersScreenState extends State<FightersScreen> {
  List<FighterForFriendDto> _allFighters = [];
  List<FighterForFriendDto> _filteredFighters = [];
  bool _loading = true;
  final TextEditingController _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _loadFighters();
    _searchController.addListener(_onSearchChanged);
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  void _loadFighters() async {
    final fighters = await Service().getAllFighters();
    setState(() {
      _allFighters = fighters;
      _filteredFighters = fighters;
      _loading = false;
    });
  }

  void _onSearchChanged() {
    final query = _searchController.text.toLowerCase();
    setState(() {
      _filteredFighters = _allFighters
          .where((f) => f.username.toLowerCase().contains(query))
          .toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Center(child: CircularProgressIndicator());
    }
    return Column(
      children: [
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 16, 16, 8),
          child: TextField(
            controller: _searchController,
            decoration: InputDecoration(
              hintText: 'Buscar por usuario...',
              prefixIcon: const Icon(Icons.search),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(18),
              ),
              contentPadding: const EdgeInsets.symmetric(
                vertical: 0,
                horizontal: 16,
              ),
            ),
          ),
        ),
        Expanded(
          child: RefreshIndicator(
            onRefresh: () async {
              _loadFighters();
            },
            child: _filteredFighters.isEmpty
                ? const Center(child: Text('No hay peleadores registrados.'))
                : ListView.separated(
                    padding: const EdgeInsets.all(16),
                    itemCount: _filteredFighters.length,
                    separatorBuilder: (_, __) => const SizedBox(height: 16),
                    itemBuilder: (context, index) {
                      final f = _filteredFighters[index];
                      return InkWell(
                        borderRadius: BorderRadius.circular(18),
                        onTap: () {
                          showDialog(
                            context: context,
                            builder: (_) => FighterDetailDialog(fighter: f),
                          );
                        },
                        child: Card(
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
                                  child: const Icon(
                                    Icons.sports_mma,
                                    color: Colors.deepPurple,
                                    size: 32,
                                  ),
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
                                      Wrap(
                                        crossAxisAlignment:
                                            WrapCrossAlignment.center,
                                        spacing: 8,
                                        runSpacing: 4,
                                        children: [
                                          Text(
                                            f.weightClass,
                                            style: const TextStyle(fontSize: 15),
                                          ),
                                        ],
                                      ),
                                      const SizedBox(height: 8),
                                      Row(
                                        children: [
                                          _StatChip(
                                            label: 'W',
                                            value: f.wins,
                                            color: Colors.green,
                                          ),
                                          const SizedBox(width: 6),
                                          _StatChip(
                                            label: 'L',
                                            value: f.losses,
                                            color: Colors.red,
                                          ),
                                          const SizedBox(width: 6),
                                          _StatChip(
                                            label: 'D',
                                            value: f.draws,
                                            color: Colors.orange,
                                          ),
                                        ],
                                      ),
                                    ],
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ),
                      );
                    },
                  ),
          ),
        ),
      ],
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

class _DetailStat extends StatelessWidget {
  final String label;
  final int value;

  const _DetailStat({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(
          '$value',
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 18,
            color: Colors.deepPurple,
          ),
        ),
        const SizedBox(height: 4),
        Text(
          label,
          style: TextStyle(color: Colors.deepPurple.shade300, fontSize: 14),
        ),
      ],
    );
  }
}
