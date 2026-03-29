function RightPanel() {
  const topPlayers = [
    { username: 'Apex_King', game: 'Valorant', rank: 'Radiant', followers: '15.2K' },
    { username: 'TacticalPro', game: 'CS2', rank: 'Global Elite', followers: '12.8K' },
    { username: 'ShadowMaster', game: 'League', rank: 'Challenger', followers: '9.5K' },
    { username: 'SpeedRunner', game: 'Fortnite', rank: 'Champion', followers: '8.3K' }
  ]

  const trendingGames = [
    'Valorant',
    'CS2',
    'League of Legends',
    'Fortnite',
    'Apex Legends'
  ]

  const recentAchievements = [
    { user: 'RumenXP', action: 'reached Immortal 3' },
    { user: 'ClutchNova', action: 'uploaded a new clip' },
    { user: 'Vexara', action: 'won 5 ranked matches in a row' },
    { user: 'EliteGamer', action: 'promoted to Diamond' },
    { user: 'NovaStrike', action: 'reached Global Elite' }
  ]

  return (
    <div className="right-panel">
      {/* Top Players Card */}
      <div className="glass-card panel-card p-4 mb-4">
        <h3 className="card-title fw-bold mb-3">Top Players</h3>
        <div className="top-players-list">
          {topPlayers.map((player, idx) => (
            <div key={idx} className="player-item">
              <div className="player-avatar-small">
                <div 
                  className="avatar-circle-xs"
                  style={{backgroundImage: `url(https://api.dicebear.com/7.x/avataaars/svg?seed=${player.username})`}}
                ></div>
              </div>
              <div className="player-info-small flex-grow-1">
                <div className="player-name text-white fw-bold">{player.username}</div>
                <div className="player-rank small text-muted">{player.game} • {player.rank}</div>
              </div>
              <div className="player-followers small text-muted">{player.followers}</div>
            </div>
          ))}
        </div>
      </div>

      {/* Trending Games Card */}
      <div className="glass-card panel-card p-4 mb-4">
        <h3 className="card-title fw-bold mb-3">Trending Games</h3>
        <div className="trending-games-list">
          {trendingGames.map((game, idx) => (
            <div key={idx} className="game-item">
              <i className="bi bi-controller"></i>
              <span className="text-light">{game}</span>
              <i className="bi bi-chevron-right ms-auto text-muted"></i>
            </div>
          ))}
        </div>
      </div>

      {/* Recent Achievements Card */}
      <div className="glass-card panel-card p-4">
        <h3 className="card-title fw-bold mb-3">Recent Achievements</h3>
        <div className="achievements-list">
          {recentAchievements.map((item, idx) => (
            <div key={idx} className="achievement-item">
              <i className="bi bi-lightning-charge"></i>
              <div className="achievement-text">
                <span className="achievement-user fw-bold">{item.user}</span>
                <span className="achievement-action text-muted ms-1">{item.action}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}

export default RightPanel
