import { useState } from 'react'

function FeedComposer() {
  const [postType, setPostType] = useState('achievement')

  return (
    <div className="glass-card composer-card p-4">
      {/* Header */}
      <div className="composer-header">
        <h3 className="fw-bold mb-4">Share Your Gaming Moment</h3>
      </div>

      {/* Composer Input Area */}
      <div className="composer-input-section">
        <div className="d-flex gap-3 mb-4">
          <div className="composer-avatar">
            <div 
              className="avatar-small" 
              style={{backgroundImage: 'url(https://api.dicebear.com/7.x/avataaars/svg?seed=rumen)'}}
            ></div>
          </div>
          <textarea
            className="form-control composer-textarea"
            placeholder="Share an achievement, match result, clip, or team search update..."
            rows="3"
          ></textarea>
        </div>

        {/* Post Type Selector */}
        <div className="composer-types mb-3">
          <div 
            className={`post-type-chip ${postType === 'achievement' ? 'active' : ''}`}
            onClick={() => setPostType('achievement')}
          >
            <i className="bi bi-star"></i> Achievement
          </div>
          <div 
            className={`post-type-chip ${postType === 'matchResult' ? 'active' : ''}`}
            onClick={() => setPostType('matchResult')}
          >
            <i className="bi bi-bar-chart"></i> Match Result
          </div>
          <div 
            className={`post-type-chip ${postType === 'clip' ? 'active' : ''}`}
            onClick={() => setPostType('clip')}
          >
            <i className="bi bi-play-circle"></i> Clip
          </div>
          <div 
            className={`post-type-chip ${postType === 'teamSearch' ? 'active' : ''}`}
            onClick={() => setPostType('teamSearch')}
          >
            <i className="bi bi-people"></i> Team Search
          </div>
        </div>

        {/* Media and Action Buttons */}
        <div className="composer-actions">
          <div className="composer-media-buttons">
            <button className="btn btn-sm btn-outline-secondary me-2">
              <i className="bi bi-image"></i> Image
            </button>
            <button className="btn btn-sm btn-outline-secondary">
              <i className="bi bi-film"></i> Video
            </button>
          </div>
          <button className="btn purple-btn px-4 py-2 fw-bold">
            <i className="bi bi-send"></i> Post
          </button>
        </div>
      </div>
    </div>
  )
}

export default FeedComposer
