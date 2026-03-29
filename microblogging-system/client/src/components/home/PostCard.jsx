function PostCard({ 
  username = 'RumenXP', 
  handle = '@rumenxp',
  avatar = 'https://api.dicebear.com/7.x/avataaars/svg?seed=rumen',
  game = 'Valorant',
  rank = 'Immortal 3',
  timestamp = '2h ago',
  content = 'Reached Immortal 3 after winning 6 of my last 7 games. Looking for a serious team to scrim with this weekend.',
  postType = 'achievement',
  stats = null,
  mediaType = null,
  likes = 342,
  comments = 28,
  views = 1240
}) {
  return (
    <div className="glass-card post-card p-4 mb-4">
      {/* Post Header */}
      <div className="post-header">
        <div className="d-flex align-items-center gap-3 mb-3">
          <div 
            className="post-avatar"
            style={{backgroundImage: `url(${avatar})`}}
          ></div>
          <div className="post-header-info flex-grow-1">
            <div className="post-name-row">
              <span className="post-name fw-bold">{username}</span>
              <span className="post-handle text-muted ms-2">{handle}</span>
              <span className="post-timestamp text-muted ms-auto small">{timestamp}</span>
            </div>
            <div className="post-meta">
              <span className="badge badge-game">{game}</span>
              <span className="badge badge-rank">{rank}</span>
              <span className={`badge badge-type badge-${postType.toLowerCase()}`}>
                <i className={`bi ${postType === 'achievement' ? 'bi-star' : postType === 'matchResult' ? 'bi-bar-chart' : postType === 'clip' ? 'bi-play-circle' : 'bi-people'}`}></i>
                {postType}
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* Post Content */}
      <div className="post-content mb-3">
        <p className="mb-0">{content}</p>
      </div>

      {/* Stats Section (if available) */}
      {stats && (
        <div className="post-stats mb-3">
          <div className="stat-item">
            <div className="stat-label">Last Match</div>
            <div className="stat-value text-success fw-bold">{stats.lastMatch}</div>
          </div>
          <div className="stat-item">
            <div className="stat-label">KDA</div>
            <div className="stat-value">{stats.kda}</div>
          </div>
          <div className="stat-item">
            <div className="stat-label">Map</div>
            <div className="stat-value">{stats.map}</div>
          </div>
        </div>
      )}

      {/* Media Placeholder (if available) */}
      {mediaType && (
        <div className="post-media-placeholder mb-3">
          {mediaType === 'image' && (
            <div className="media-image-placeholder">
              <i className="bi bi-image"></i>
              <span>Image</span>
            </div>
          )}
          {mediaType === 'video' && (
            <div className="media-video-placeholder">
              <i className="bi bi-play-circle"></i>
              <span>Video Clip</span>
            </div>
          )}
        </div>
      )}

      {/* Post Actions */}
      <div className="post-actions">
        <div className="action-item">
          <i className="bi bi-heart"></i>
          <span>{likes}</span>
        </div>
        <div className="action-item">
          <i className="bi bi-chat"></i>
          <span>{comments}</span>
        </div>
        <div className="action-item">
          <i className="bi bi-eye"></i>
          <span>{views}</span>
        </div>
        <div className="action-item ms-auto">
          <i className="bi bi-share"></i>
          <span>Share</span>
        </div>
      </div>
    </div>
  )
}

export default PostCard
